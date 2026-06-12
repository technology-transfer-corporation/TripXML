# Performance analysis — TripXML .NET 10 migration

> **Read this first.** The mechanism sections below (HTTP transport, hosting,
> runtime, XSLT, serialization) describe **expected** effects of the migration,
> grounded in code and Microsoft documentation — they are not measurements.
> The only measured numbers in this document are in
> [Measured (2026-06-12)](#measured-2026-06-12-new-stack-only-localhost-no-baseline),
> taken on the **new stack only**, on localhost, in Release configuration.
> **No legacy-side baseline exists.** Nothing here supports a claim that the
> new stack is faster or slower than legacy production end to end.

Cross-references: [migration-report.md](migration-report.md) (what was changed),
[architecture.md](architecture.md) (how the host is wired),
[development-guide.md](development-guide.md) (how to build/run),
[ADR-0001](adr/0001-corewcf-over-rest-rewrite.md),
[ADR-0002](adr/0002-runtime-xslt-compilation.md),
[ADR-0003](adr/0003-handrolled-soap-clients.md). Doc index: [README.md](README.md).

## Summary

| Area | Legacy (.NET Framework 4.8 / IIS) | New (.NET 10) | Status |
|---|---|---|---|
| Outbound HTTP to GDS providers | Per-call `HttpWebRequest`, private reflection on `_HttpResponse` to read error bodies | Shared `HttpClient` over `SocketsHttpHandler`, pooled connections | Expected improvement, not measured |
| Hosting | IIS + System.Web ASMX | Kestrel + CoreWCF middleware, 104 `BasicHttpBinding` endpoints | Expected improvement, not measured |
| Runtime | .NET Framework 4.8 JIT/GC | .NET 10: tiered compilation, dynamic PGO, Server GC | Expected improvement, not measured |
| XSLT | xsltc-precompiled assemblies (compile cost at build time) | Runtime compile at first use + in-process cache | **Tradeoff**, first-use cost measured |
| XmlSerializer | sgen-style pre-generation available at build time | First-use codegen at runtime, per process | **Regression at cold start**, measured |
| Response compression | `CompressionExtension` SoapExtension (per-request gzip) | **Not configured** | **Regression**, see [Known regressions](#known-performance-regressions-and-risks) |

Build/test state at the time of writing (branch `docs/net10-migration-report`,
HEAD `e434fed`, .NET SDK 10.0.300, Windows 11 Pro 10.0.26200, win-x64):
`dotnet build TripXML.slnx -c Release` → 0 errors, 839 warnings
(analyzer-level, e.g. CA2200 re-throw), 39.7 s;
`dotnet test src/TripXMLMain/tests/TripXML.XsltTests -c Release` → 24/24 passed in 2 s.

## HTTP transport to GDS providers (expected)

The legacy adapters created a new `HttpWebRequest` per call. On non-success
status codes, `GetResponse()` threw, and the Sabre/Worldspan/Amadeus adapters
recovered the SOAP fault body via private reflection on `HttpWebRequest`'s
internal `_HttpResponse` field. Both patterns are gone from every live
transport path; the one residual is src/Galileo/ttHttpWebClient.cs (legacy
`HttpWebRequest`, no in-tree callers — recorded in
[ADR-0003](adr/0003-handrolled-soap-clients.md)). Each provider adapter now
holds one process-wide `HttpClient` over a `SocketsHttpHandler`:

| Adapter | File | Pooled connection lifetime | Request timeout | Decompression |
|---|---|---|---|---|
| Sabre | src/Sabre/ttHttpWebClient.cs | `TimeSpan.FromMinutes(5)` | 60000 ms per request via `CancellationTokenSource` (`REQUEST_TIMEOUT_MS`, line 22) | GZip |
| Worldspan | src/Worldspan/ttHttpWebClient.cs | `TimeSpan.FromMinutes(5)` | client `Timeout.InfiniteTimeSpan`; legacy 180 s applied per request (comment, lines 22–24) | GZip |
| Amadeus | src/Amadeus/ttHttpWebClient.cs | `TimeSpan.FromMinutes(5)` | 90000 ms client-wide (`CreateHttpClient`, line 74) | GZip + Deflate |
| TravelPort | src/TravelPort/ttHttpWebClient.cs | `TimeSpan.FromMinutes(5)` | none configured — `HttpClient` default 100 s applies | GZip |
| Galileo | src/Galileo/Classes/GalileoSoapClient.cs | `TimeSpan.FromMinutes(5)` | `DefaultTimeoutMs` = 100000 ms (line 48), per-request token | GZip |

Expected effects, with Microsoft documentation:

- **Connection reuse, no socket exhaustion.** A single long-lived `HttpClient`
  pools connections inside its handler and reuses them across requests;
  per-request client/handler creation is the documented anti-pattern that
  exhausts sockets under load
  (<https://learn.microsoft.com/dotnet/fundamentals/runtime-libraries/system-net-http-httpclient#instancing>).
- **DNS freshness without per-call setup cost.** `PooledConnectionLifetime`
  bounds how long a pooled connection lives so DNS changes are picked up when
  the connection is recycled — the documented pattern for static clients
  (<https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#dns-behavior>,
  <https://learn.microsoft.com/dotnet/api/system.net.http.socketshttphandler.pooledconnectionlifetime?view=net-10.0>).
  All five adapters use `FromMinutes(5)` with a code comment "pooled connection
  lifetime keeps DNS fresh".
- **gzip handled by the handler.** Setting `AutomaticDecompression` makes the
  handler add `Accept-Encoding` to every outgoing request and decompress
  responses based on `Content-Encoding`
  (<https://learn.microsoft.com/dotnet/api/system.net.http.socketshttphandler.automaticdecompression?view=net-10.0>).
  The Amadeus adapter's comment (src/Amadeus/ttHttpWebClient.cs lines 55–57)
  records that legacy code sent `Accept-Encoding: gzip,deflate` and decompressed
  manually; the deleted manual path survives in src/Amadeus/ttHttpWebClient.cs.bkp.
- **Error bodies without reflection.** With `HttpClient`, a non-success
  response still exposes its content stream, so the SOAP fault payload is read
  on the normal path (src/Sabre/ttHttpWebClient.cs lines 75–78,
  src/Worldspan/ttHttpWebClient.cs line 87: "this replaces the legacy
  reflection on `_HttpResponse`"). The general migration mapping is documented at
  <https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-migrate-from-httpwebrequest>.

None of this was measured against the legacy adapters. The per-request timeout
values were deliberately kept at their legacy values (60000/90000/180000 ms),
so timeout behavior should be unchanged. Rationale for hand-rolling these
clients instead of generating WCF proxies: [ADR-0003](adr/0003-handrolled-soap-clients.md).

## Hosting pipeline (expected)

Legacy: IIS worker process, System.Web ASMX handlers. New: Kestrel with CoreWCF
middleware. src/wsTripXML/Program.cs registers all 104 services from
`ServiceRoutes.All` (src/wsTripXML/Code/Generated/ServiceRoutes.g.cs, 104
entries, verified) on per-service `BasicHttpBinding` instances — SOAP 1.1 over
HTTP(S), matching the ASMX wire format (comment at Program.cs lines 46–48).

- Kestrel is the default, cross-platform ASP.NET Core server, "optimized to
  handle a large number of concurrent connections efficiently"
  (<https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-10.0>),
  and Microsoft documents it as providing "the best performance and memory
  utilization" of the ASP.NET Core server options
  (<https://learn.microsoft.com/aspnet/core/fundamentals/servers/?view=aspnetcore-10.0>).
- Self-hosted Kestrel removes the IIS module hop entirely; if IIS is later
  reintroduced as a reverse proxy, the in-process vs out-of-process tradeoffs
  are documented at the same URL.

Expected effect only; no load comparison against the IIS deployment was run.
The decision to keep SOAP via CoreWCF instead of rewriting to REST is
[ADR-0001](adr/0001-corewcf-over-rest-rewrite.md).

## Runtime (expected)

The jump from .NET Framework 4.8 to .NET 10 brings runtime-level changes that
apply to this workload without code changes. None were measured here in
isolation; each claim cites Microsoft documentation:

- **Tiered compilation** — methods are first jitted quickly, then recompiled
  optimized in the background; on by default since .NET Core 3.0
  (<https://learn.microsoft.com/dotnet/core/runtime-config/compilation#tiered-compilation>).
- **Dynamic PGO** — the JIT instruments tier-0 code and uses the observed
  profile when producing optimized code; enabled by default since .NET 8
  (<https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8/runtime>).
- **Server GC** — "The default GC for ASP.NET Core apps", optimized for
  throughput on servers, vs workstation GC on .NET Framework hosts that did not
  opt in (<https://learn.microsoft.com/aspnet/core/performance/memory?view=aspnetcore-10.0>,
  <https://learn.microsoft.com/dotnet/standard/garbage-collection/workstation-server-gc>).
  Note Server GC trades a larger working set for throughput; the memory page
  above shows the difference and how to switch with `ServerGarbageCollection`.
- **Span-based BCL** — `Span<T>`/`Memory<T>` let library code avoid copying and
  heap allocation (<https://learn.microsoft.com/dotnet/standard/memory-and-spans/>);
  this codebase benefits indirectly through the BCL (string, XML, HTTP paths),
  not through its own code, which remains largely allocation-heavy
  (`XmlDocument`, string concatenation in the adapters).

## XSLT engine — a tradeoff, not a win

This is the one subsystem where the migration **moved a cost from build time to
run time**. Do not read this section as "XSLT is faster now"; it is not a
claim this repo can make.

- **Legacy:** stylesheets were precompiled to assemblies with xsltc.exe, so the
  compile cost was paid at build time and `XslCompiledTransform.Load(Type)`
  loaded ready code (<https://learn.microsoft.com/dotnet/standard/data/xml/xslt-compiler-xsltc-exe>).
  That option no longer exists: "assemblies compiled using xsltc.exe cannot be
  referenced in .NET"
  (<https://learn.microsoft.com/dotnet/standard/data/xml/how-to-perform-an-xslt-transformation-by-using-an-assembly>).
- **New:** `CoreLib.TransformXML` (src/TripXMLMain/CoreLib.cs) compiles each
  stylesheet from source at first use — `XslCompiledTransform.Load` "Compiles
  the style sheet"
  (<https://learn.microsoft.com/dotnet/api/system.xml.xsl.xslcompiledtransform.load?view=net-10.0>) —
  and caches the result in a
  `ConcurrentDictionary<string, XslCompiledTransform>` (`_xslCache`, lines
  25–26). The intent is stated in the code comment at lines 51–52:

  > Stylesheets are loaded from source and compiled once per process; the cache
  > replicates the performance profile of the retired xsltc-precompiled assemblies.

- The compile cost is paid **once per process per stylesheet** (and again after
  `CoreLib.ClearXslCache()`, the wsRefreshMem hook at lines 82–85, empties the
  cache). Measured first-use costs are in the
  [Measured](#measured-2026-06-12-new-stack-only-localhost-no-baseline) section:
  2–78 ms per stylesheet, scaling with stylesheet size. There are 553 `.xsl`
  files under src/XSLs; only stylesheets actually requested get compiled.
- **What was actually won is operational, not throughput:** no fleet of
  precompiled XSLT DLLs to build, version, and deploy alongside the host; and
  commit `4cda4b7` removed the `msxsl:script` blocks from 27 stylesheets
  (28 files changed, +1/−351 — the extra file is an import-path fix in
  src/XSLs/Galileo/Galileo_QueueRS.xsl), eliminating the CodeDom script-compile
  dependency that .NET no longer supports (script blocks are .NET
  Framework-only, per the xsltc page above). The new engine loads with
  `XsltSettings(enableDocumentFunction: true, enableScript: false)`
  (CoreLib.cs line 56).
- Steady-state transform throughput is **expected to be comparable** — both
  stacks execute `XslCompiledTransform` — but this was not measured with
  realistic payloads. Full decision record: [ADR-0002](adr/0002-runtime-xslt-compilation.md).

## Serialization (expected, first-use cost measured)

CoreWCF service contracts here use `XmlSerializer`-style serialization for ASMX
wire compatibility. When no pre-generated serialization assembly exists,
"XmlSerializer generates serialization code and a serialization assembly for
each type every time an application is run"
(<https://learn.microsoft.com/dotnet/standard/serialization/xml-serializer-generator-tool-sgen-exe>).
On .NET Framework, sgen.exe could pre-generate those assemblies at build time
(same URL; Microsoft scopes the pre-generated assemblies to clients and manual
serialization, not the server side of a web service). The modern equivalent is
the `Microsoft.XmlSerializer.Generator` package
(<https://learn.microsoft.com/dotnet/core/additional-tools/xml-serializer-generator>);
it is **not** wired into this solution (no reference anywhere under src/,
verified), so first-use serializer generation happens at runtime, per process.

This is directly visible in the measurements below: the first `wsPing` SOAP
call took 158.43 ms while the steady-state median is 2.02 ms — the gap is
dominated by first-use serializer codegen plus first-request pipeline warmup.

## Measured (2026-06-12, new stack only, localhost, no baseline)

Environment: branch `docs/net10-migration-report`, HEAD `e434fed`,
.NET SDK 10.0.300 (MSBuild 18.6.3), Windows 11 Pro 10.0.26200, RID win-x64,
Release build, placeholder configuration (src/wsTripXML/appsettings.json is
placeholders-only), HTTP over the localhost loopback.

**There is no legacy-side baseline.** These numbers characterize the new stack
in isolation; they cannot be compared against legacy production, and loopback
latencies say nothing about network or database-bound endpoints.

### Host cold start

Process start until the first HTTP 200 from `/wsPing.asmx?wsdl`, with all 104
CoreWCF endpoints registered. Poll granularity 25 ms; the values are upper
bounds at that granularity.

| Run | Cold start (ms) |
|---|---|
| First | 1106 |
| Second | 1113 |
| Third | 1159 |

### wsPing SOAP round-trip

SOAP POST to operation `ping` (`public string ping(int Delay)`,
src/wsTripXML/Code/wsPing.asmx.cs line 144) with `Delay=-1`, which returns
without waiting. Client: `Invoke-WebRequest` in the same machine's PowerShell.

| Sample | Round-trip (ms) |
|---|---|
| First call (includes XmlSerializer first-use codegen) | 158.43 |
| Second call | 11.39 |
| Steady state, median over 20 calls | 2.02 |
| Steady state, min over 20 calls | 1.68 |
| Steady state, max over 20 calls | 4.01 |

### XSLT compile (first use) vs cache hit

In-process harness calling `CoreLib.TransformXML`, JIT pre-warmed, with the
trivial non-matching input `<root/>`.

| Stylesheet | Size | First call (compile) | Cached median |
|---|---|---|---|
| src/XSLs/Sabre/v03_Sabre_PNRReadRS.xsl | 254 KB | 78.28 ms | 0.016 ms |
| src/XSLs/Aggregation/Markups_LowFareRS.xsl | 31 KB | 16.29 ms | 0.006 ms |
| src/XSLs/Aggregation/Aggregation_AirAvailRS.xsl | 2 KB | 2.28 ms | 0.003 ms |

**Mandatory caveat:** the cached medians measure cache-hit overhead plus a
transform over trivial input that matches no templates — they are **not**
realistic payload transform costs. The valid headline is only this: stylesheet
compile cost (2–78 ms, scaling with size) is paid once per process per
stylesheet; the retired xsltc approach paid it at build time instead.

## Known performance regressions and risks

Named plainly, with the evidence:

1. **Response compression is not configured on the new host.** The legacy
   stack applied per-request gzip through the `CompressionExtension` ASMX
   `SoapExtension` ([MIGRATION_STATUS.md](../MIGRATION_STATUS.md) documents it
   and recommended replacing it with response-compression middleware during the
   host migration). The conversion stripped the `[CompressionExtension()]`
   attributes (tools/Convert-Services.ps1 line 31) and **no replacement was
   added**: src/wsTripXML/Program.cs contains no
   `AddResponseCompression`/`UseResponseCompression`. Clients that previously
   negotiated compressed SOAP responses now receive uncompressed payloads —
   more bytes on the wire for every large response. The fix is ASP.NET Core's
   Response Compression Middleware
   (<https://learn.microsoft.com/aspnet/core/performance/response-compression?view=aspnetcore-10.0>);
   note its `EnableForHttps` security caveat (CRIME/BREACH) on that page before
   enabling it for HTTPS.
2. **Cold-start costs moved from build/deploy time to first-request time.**
   Two formerly build-time artifacts are now generated at runtime, per process:
   XmlSerializer serialization assemblies (measured: 158.43 ms first `wsPing`
   call vs 2.02 ms steady state) and compiled stylesheets (measured: up to
   78.28 ms for the largest measured stylesheet). Every process restart, deploy,
   or `CoreLib.ClearXslCache()` call re-incurs these costs on the requests that
   hit them first. Mitigations if this matters in production: post-deploy
   warmup requests against the hot endpoints, and/or wiring up
   `Microsoft.XmlSerializer.Generator`
   (<https://learn.microsoft.com/dotnet/core/additional-tools/xml-serializer-generator>).
3. **Buffered messages up to 64 MB.** Every endpoint is registered with
   `MaxReceivedMessageSize = 64_000_000` and `MaxBufferSize = 64_000_000`
   (src/wsTripXML/Program.cs lines 51–56 for HTTP, 63–68 for HTTPS) in
   `BasicHttpBinding`'s default buffered transfer mode, far above the binding
   default. In buffered mode the entire message is held in memory while it is
   processed, and the quota bounds per-message memory only "to within a
   constant factor" — deserialization allocates on top of the raw buffer
   (<https://learn.microsoft.com/dotnet/framework/wcf/feature-details/large-data-and-streaming>,
   <https://learn.microsoft.com/dotnet/framework/wcf/feature-details/security-considerations-for-data>).
   Concurrent large requests multiply this. This mirrors the legacy
   configuration intent but deserves a staging memory profile before high load.
4. **Ten endpoints behave differently from legacy production by design.**
   Commit `bf710ba` (the host migration, 283 files, +6533/−8055) documents
   fixing endpoints that return HTTP 500 on legacy prod. Performance
   comparisons against legacy for those endpoints are meaningless — legacy
   fails them. See [migration-report.md](migration-report.md).

## Future measurement plan (proposed, out of scope)

None of this exists yet; listed so the gaps are explicit:

- **BenchmarkDotNet harness** for `CoreLib.TransformXML` (realistic GDS
  response payloads, not `<root/>`) and for XmlSerializer round-trips of the
  heaviest contracts.
- **Load test** `wsPing` (floor) and one heavy endpoint such as `wsAirAvail`
  (src/wsTripXML/Code/wsAirAvail.asmx.cs) with realistic payloads and
  concurrency, on server-class hardware rather than a developer workstation.
- **dotnet-counters baseline in staging**: GC pause/heap counters under Server
  GC with the 64 MB buffered bindings, thread pool, and Kestrel connection
  counters.
- **Side-by-side against legacy prod** while it still runs: same payloads to
  both stacks, compare latency distributions and response sizes (the response
  size delta quantifies regression item 1 above).

## Related documents

- [migration-report.md](migration-report.md) — per-repo migration commits and counts
- [architecture.md](architecture.md) — host, routing, and adapter structure
- [development-guide.md](development-guide.md) — build, test, run commands
- [adr/README.md](adr/README.md) — all decision records
- [monorepo-consolidation-design.md](monorepo-consolidation-design.md) — repo assembly
- [ado-retirement.md](ado-retirement.md) — legacy repo retirement checklist
- [MIGRATION_STATUS.md](../MIGRATION_STATUS.md) — pre-migration audit (root)
