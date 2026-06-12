# TripXML .NET 10 Migration Report

Status: migration complete and committed. Executed 2026-06-11, landed in the monorepo as 9 per-project
commits dated 2026-06-12. Verification evidence in this report was captured 2026-06-12 at HEAD `e434fed`.

This is the comprehensive record of the migration from .NET Framework 4.x (ASMX on IIS) to .NET 10
(CoreWCF on Kestrel). For the architecture of the resulting system see [architecture.md](architecture.md);
for the measurement methodology and full performance discussion see [performance.md](performance.md);
for day-to-day workflows and the living gap list see [development-guide.md](development-guide.md).
The decision rationale lives in the [ADR set](adr/README.md).

## 1. Executive summary

Nine projects moved in one coordinated change-set: the hub library **TripXMLMain**, five GDS adapters
(**Amadeus**, **Galileo**, **Sabre**, **TravelPort**, **Worldspan**), the utility library **TripXMLTools**,
the stylesheet tree **XSLs**, and the SOAP host **wsTripXML**. The host went from a System.Web ASMX
web application on IIS to a self-hosted CoreWCF application on Kestrel (`src/wsTripXML/Program.cs`),
serving the same 104 `.asmx` routes with the same SOAP 1.1 document/literal wire format.

Headline results (all measured 2026-06-12, .NET SDK 10.0.300, Windows 11 Pro 10.0.26200, win-x64):

- `dotnet build TripXML.slnx -c Release`: **0 errors**, 839 warnings (analyzer-level, e.g. CA2200
  re-throw patterns inherited from the VB conversion), 39.7 s.
- `dotnet test src/TripXMLMain/tests/TripXML.XsltTests -c Release`: **24/24 passed** in 2 s.
- **104/104 CoreWCF endpoints registered and served**: `ServiceRoutes.All` in
  `src/wsTripXML/Code/Generated/ServiceRoutes.g.cs` has exactly 104 entries; host cold start to first
  HTTP 200 from `/wsPing.asmx?wsdl` was 1106/1113/1159 ms over 3 runs (new stack only; there is no
  legacy-side baseline for comparison).
- 10 endpoints that returned HTTP 500 on legacy production now work (XML serializer type-name
  collisions fixed during contract generation; commit `bf710ba`).

The before-state is documented in the 2026-06-10 pre-migration audit, [MIGRATION_STATUS.md](../MIGRATION_STATUS.md).
In short: of 17 Azure DevOps repos, essentially nothing was migrated. The only genuine prior attempt,
the `wsTripXML@converted` branch, was a complete VB→C# translation that **could not compile** — no
entry point, 100+ code-behinds still on `System.Web.Services` (which does not exist on modern .NET),
and all 7 sibling ProjectReferences still targeting net48. The dependency hub, TripXMLMain, depended
on ~285 precompiled .NET-Framework-only XSLT assemblies (xsltc output, some carrying VB
`msxsl:script` blocks) — identified in the audit as the single hardest technical problem. Production
credentials were committed throughout (`Web.config`, `Tables.zip`, build scripts); secret removal and
history scrubbing were handled by the monorepo consolidation
([monorepo-consolidation-design.md](monorepo-consolidation-design.md), [ado-retirement.md](ado-retirement.md))
and commit `240561a`, which deleted `Web.config`/`Web.Debug.config`/`Web.Release.config` from the
working tree and reduced `src/wsTripXML/appsettings.json` to placeholders.

## 2. Before/after matrix

| Project | Old stack | New stack | Commit | Shortstat |
|---|---|---|---|---|
| TripXMLMain | legacy csproj net48; ~285 refs to xsltc-precompiled XSLT DLLs; `HttpApplicationState`; System.Data.SqlClient | SDK csproj net10.0; runtime `XslCompiledTransform` + `urn:ttVB` extension object; `AppState` over `IMemoryCache`; Microsoft.Data.SqlClient; new test project | `9b8f052` | 24 files, +435 −1014 |
| XSLs | 553 `.xsl` sources, 27 with VB `msxsl:script` blocks, compiled by xsltc.exe at build time | same 553 sources loaded at runtime; `msxsl:script` blocks removed (functions now live in `src/TripXMLMain/TtVbXsltFunctions.cs`) | `4cda4b7` | 28 files, +1 −351 |
| TripXMLTools | legacy csproj net48; `WebConfigurationManager` (System.Web) | SDK csproj net10.0; `modCore.config` bridge; `TripXMLLoad.BuildDecodingDataViews` publishes DataViews to `AppState` | `141e022` | 6 files, +66 −352 |
| Sabre | legacy csproj net48; `HttpWebRequest` + private reflection on FW-internal `_HttpResponse` to read error bodies | SDK csproj net10.0; shared `HttpClient`/`SocketsHttpHandler`; non-2xx bodies read on the normal path | `487d7a1` | 3 files, +126 −314 |
| Worldspan | legacy csproj net48; same `_HttpResponse` reflection hack as Sabre | SDK csproj net10.0; `HttpClient` transport | `9e9c590` | 3 files, +132 −317 |
| TravelPort | legacy csproj net48; `HttpWebRequest` | SDK csproj net10.0; `ttHttpWebClient` rewritten on shared `HttpClient`, public signatures unchanged | `85eafde` | 3 files, +52 −177 |
| Galileo | legacy csproj net48 + GalileoHttpUtil subproject; 4 ASMX Web Reference proxies (`SoapHttpClientProtocol`) | SDK csproj net10.0; hand-rolled `GalileoSoapClient` with byte-verified envelope parity; GalileoHttpUtil deleted | `21a86e9` | 11 files, +451 −1006 |
| Amadeus | legacy csproj net48; `HttpContext.Current` host coupling; process-wide TLS-validation bypass via `ServicePointManager`; `wsBlackList` Web Reference proxy | SDK csproj net10.0; `HttpClient` transport; per-handler cert gate (`AmadeusSkipCertValidation`); hand-rolled `BlackListClient`; `HttpContext` references gone | `b569756` | 7 files, +469 −589 |
| wsTripXML | legacy Web-app vbproj→csproj net48; 104 ASMX endpoints on System.Web; `Web.config`; no entry point on the converted branch | `Microsoft.NET.Sdk.Web` net10.0; CoreWCF/Kestrel host with DI; 104 generated contracts; `appsettings.json` | `bf710ba` | 283 files, +6533 −8055 |

## 3. Per-project detail

### 3.1 TripXMLMain (`9b8f052`)

The hub library and the long pole. The legacy design loaded ~285 xsltc-precompiled XSLT assemblies —
a build artifact class that cannot exist on modern .NET. The replacement is runtime compilation with
process-lifetime caching in `src/TripXMLMain/CoreLib.cs`:

- `CoreLib.TransformXML` resolves a stylesheet path and compiles it once via
  `_xslCache.GetOrAdd(...)` — a `ConcurrentDictionary<string, XslCompiledTransform>` (lines 25-26) —
  with `XsltSettings(enableDocumentFunction: true, enableScript: false)`.
- Every transform registers the `urn:ttVB` extension object
  (`args.AddExtensionObject(TtVbXsltFunctions.Namespace, TtVbXsltFunctions.Instance)`), replacing the
  VB `msxsl:script` blocks. `src/TripXMLMain/TtVbXsltFunctions.cs` documents the contract: XSLT binds
  extension functions by reflected method name, case-sensitively, so names and signatures must match
  the retired script functions exactly.
- `CoreLib.ClearXslCache()` drops compiled stylesheets so edited `.xsl` files are picked up.

Other changes: `src/TripXMLMain/AppState.cs` added (see §4 and [ADR-0004](adr/0004-appstate-facade.md));
`src/TripXMLMain/modCore.cs` gained a settable `config` `NameValueCollection` property (lines 24-25,
commented "Populated by the host at startup from IConfiguration (replaces ConfigurationManager.AppSettings)") ([ADR-0005](adr/0005-modcore-config-bridge.md)); 13 tracked `Xsl/**/*.dll` xsltc binaries
deleted; `src/TripXMLMain/tests/TripXML.XsltTests/` added — 24 tests across
`XsltCompileAllTests.cs` (compile every stylesheet), `GoldenTransformTests.cs` (golden-output
transforms), and `TtVbXsltFunctionsTests.cs` (value tables for the extension functions, culture-pinned
to en-US). Decision record: [ADR-0002](adr/0002-runtime-xslt-compilation.md).

### 3.2 XSLs (`4cda4b7`)

`msxsl:script` blocks removed from 27 stylesheets (28 files changed, +1/−351 — the extra file is an
import-path fix in `src/XSLs/Galileo/Galileo_QueueRS.xsl`, which carried an absolute dev-machine
import path). The 553 `.xsl` files under `src/XSLs` are now runtime inputs, not build inputs; the
xsltc.exe toolchain and the 24 xcopy `.bat` deployment scripts the audit flagged are dead.
Stylesheets that referenced script functions now resolve them through the `urn:ttVB` namespace bound
at transform time (verified usage in e.g. `src/XSLs/Galileo/v03_Galileo_PNRReadRS.xsl`,
`src/XSLs/Worldspan/v03_Worldspan_PNRReadRS.xsl`).

### 3.3 TripXMLTools (`141e022`)

`WebConfigurationManager` (System.Web) reads replaced by the `modCore.config` bridge
(`src/TripXMLTools/SettingsService.cs`). `src/TripXMLTools/TripXMLLoad.cs` gained
`BuildDecodingDataViews`, which publishes the `ttAirports`/`ttAirlines`/`ttEquipments` decoding
DataViews into `AppState` at startup (invoked from `TripXMLRuntime.Initialize`, see §3.9). The
orphaned `TripXMLLog.cs` (137 lines) and `App.config` were deleted.

### 3.4 Sabre (`487d7a1`)

The legacy transport used `HttpWebRequest` and, on error, a private-reflection hack against the
Framework-internal `_HttpResponse` type to read SOAP fault bodies off `WebException` — a pattern that
silently breaks on modern .NET. `src/Sabre/ttHttpWebClient.cs` now uses a shared
`HttpClient`/`SocketsHttpHandler` and reads the body regardless of status code; per the in-code
comment: "non-success responses (e.g. Sabre SOAP fault bodies on HTTP 500) still carry the payload
the caller needs. The legacy code reached it through a reflection hack on WebException; with
HttpClient this is simply the normal path."

### 3.5 Worldspan (`9e9c590`)

Same shape as Sabre: SDK csproj, `src/Worldspan/ttHttpWebClient.cs` rewritten on `HttpClient`,
`_HttpResponse` reflection removed.

### 3.6 TravelPort (`85eafde`)

Smallest diff of the adapters (+52 −177). `src/TravelPort/ttHttpWebClient.cs` rewritten on the shared
`HttpClient`/`SocketsHttpHandler` pattern with unchanged public signatures, so call sites in
wsTripXML and TripXMLMain did not move.

### 3.7 Galileo (`21a86e9`)

Largest adapter rewrite. The four generated ASMX Web Reference proxies
(wsGalileoProd/wsGalileoCopy XmlSelect, wsGalileoProdIV/wsGalileoCopyIV ImageViewer — all
`SoapHttpClientProtocol`, unavailable on net10.0) are replaced by one hand-rolled client,
`src/Galileo/Classes/GalileoSoapClient.cs` (408 lines). Envelope parity is byte-verified — from the
class doc comment: "The request envelopes are byte-identical to what the legacy proxies produced
(verified by capturing the generated .NET Framework 4.8 proxies' output against a loopback
listener): UTF-8 without BOM, no indentation, namespace declarations in the order soap/xsi/xsd,
wrapped document/literal parameters in declared order, and XmlElement payloads nested inside their
wrapper member elements." Response handling, SOAPAction prefixes
(http vs https per system), the legacy `MS Web Services Client Protocol` user agent, and SOAP-fault
exception semantics are preserved per call site. The entire `GalileoHttpUtil` subproject
(`EnhancedSoapHttpClientProtocol.cs`, `GZipHttpWebRequest.cs`, `GZipHttpWebResponse.cs`) was deleted;
gzip is now `AutomaticDecompression` on the handler. Decision record:
[ADR-0003](adr/0003-handrolled-soap-clients.md).

### 3.8 Amadeus (`b569756`)

`src/Amadeus/ttHttpWebClient.cs` rewritten on `HttpClient`/`SocketsHttpHandler` (654 lines changed):
manual gzip/deflate handling replaced by `AutomaticDecompression`, the `HttpContext.Current` host
coupling removed (zero `HttpContext` references remain under `src/Amadeus`), and the legacy
**process-wide** TLS-validation bypass (`ServicePointManager.ServerCertificateValidationCallback`)
narrowed to the Amadeus handler only, gated by the `AmadeusSkipCertValidation` setting — see §6 for
why the gate's default is a sharp edge. The `wsBlackList` Web Reference proxy is replaced by
`src/Amadeus/BlackListClient.cs`, which hand-produces "the identical SOAP 1.1 document/literal
wrapped envelope described by Web References\wsBlackList\wsFlightBlackList.wsdl" (class doc comment).

### 3.9 wsTripXML (`bf710ba`)

The host. 283 files changed (+6533 −8055): 111 added (notably `Program.cs`, `appsettings.json`, and
the 105 files under `Code/Generated/`), 171 modified (code-behinds mechanically converted by
`tools/Convert-Services.ps1`), 1 deleted (`Contract.cs`).

- **Hosting**: `src/wsTripXML/Program.cs` is a `WebApplication` that registers CoreWCF
  (`AddServiceModelServices`/`AddServiceModelMetadata`), iterates `ServiceRoutes.All`, and adds one
  `BasicHttpBinding` endpoint per service at its legacy `/*.asmx` route. WSDL is served via
  `ServiceMetadataBehavior` with `HttpGetEnabled`/`HttpsGetEnabled`. See
  [ADR-0001](adr/0001-corewcf-over-rest-rewrite.md) for why CoreWCF instead of a REST rewrite.
- **Service classes**: each ASMX code-behind became a plain partial class with a `modMain` DI
  constructor; the contract metadata lives in the generated interfaces. On disk: 103 `.asmx.cs`
  code-behind files plus 3 plain `.cs` service classes (`Code/wsStoredFareBuild.cs`,
  `Code/wsStoredFareBuild_v03.cs`, `Code/wsStoredFareUpdate.cs`); `wsCruiseCabinUnUnhold` is declared
  inside `Code/wsCruiseCabinUnhold.asmx.cs`. Together these back the 104 routed services. A
  `Compile Remove` block in `src/wsTripXML/wsTripXML.csproj` (lines 33-46) excludes the 4-byte
  root `wsAuthorization.asmx.cs`, the Phase-10 ASPX pages, the fully commented-out
  `Code/Global.asax.cs`, and `My Project/**` VB leftovers.
- **Startup glue**: `src/wsTripXML/Code/Classes/TripXMLRuntime.cs` replaces
  `Global.asax Application_Start` — it pins culture, populates the `modCore` bridges, initializes
  `AppState`, and runs the legacy `modMain.TripXMLStartUp()` table load plus
  `TripXMLTools.TripXMLLoad.BuildDecodingDataViews()` in the background after startup, deliberately
  swallowing failures so "the host must come up even when Hasura/SQL are unreachable" (in-code
  comment, lines 67-68).
- **Serializer fixes**: generating all 104 contracts surfaced XML type-name collisions that the
  per-endpoint ASMX model had masked. Fixing them repaired **10 endpoints that returned HTTP 500 on
  legacy production** (documented in the `bf710ba` commit message). `tools/ContractScan` exists to
  catch the whole class of error in one run (§5).
- **SOAP logging**: the legacy ASMX `SoapExtension` for request/fault logging was ported to a CoreWCF
  `IDispatchMessageInspector` registered through `SoapRequestInspectorBehavior`
  (`src/wsTripXML/Code/Classes/cSoapRQ.cs`, line 83).

## 4. Wire-compatibility mechanisms

The non-negotiable constraint: existing SOAP clients must not notice the platform change. Five
mechanisms enforce this, all visible in the generated contracts (example used below:
`src/wsTripXML/Code/Generated/wsPing.g.cs`).

**Doc/literal via XmlSerializer.** Every contract interface carries
`[XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]`
(wsPing.g.cs line 10) — ASMX used `XmlSerializer`, not `DataContractSerializer`, and the two produce
different XML for the same types.

**TripXML SOAP header via MessageContract.** ASMX services received the `TripXML` credential header
through a `SoapHeader` field. The generated request wrapper reproduces it as a
`[MessageHeader] public TripXML TripXML;` member on a `[MessageContract(IsWrapped = true)]` class
(wsPing.g.cs lines 24-31), and the generated partial copies it to the legacy `tXML` field before
delegating to the original method body.

**`{m}Result` naming.** ASMX named the response wrapper's body element `{method}Result`. The
generated response MessageContracts preserve this exactly — e.g.
`public wmPingOut.TXML_PingRS wmPingResult;` (wsPing.g.cs line 38).

**Per-endpoint binding namespace.** ASMX emitted each service's WSDL binding in the service's own
namespace; WCF defaults everything to `tempuri.org`. `Program.cs` therefore builds a fresh
`BasicHttpBinding` per service with `Namespace = contractNs` read from the service's
`ServiceContractAttribute` (lines 49-56) — per the in-code comment: "Per-service binding instances so
the WSDL binding lands in the service namespace (ASMX kept everything in one namespace; WCF would
default to tempuri.org)."

**RequestElement routing.** Legacy services used
`SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)`: the operation is
selected by the first SOAP body element, not by SOAPAction, so clients sending empty or non-standard
SOAPAction values worked. `src/wsTripXML/Code/Classes/RequestElementRouting.cs` reproduces this with
`RequestElementRoutingBehavior` (an `IServiceBehavior` registered in DI, applied to every host) which
replaces each endpoint's `ContractFilter` and `OperationSelector`. The key comment, verbatim:

> The default ContractFilter matches on SOAPAction and rejects empty/odd actions before operation
> selection ever runs — ASMX RequestElement routing ignores the action entirely, so accept
> everything here and let the operation selector route by body element.

Hence `endpointDispatcher.ContractFilter = new MatchAllMessageFilter();` (line 42) plus
`RequestElementOperationSelector`, which buffers the message, peeks the first body element's local
name, and returns it as the operation name (wrapped doc/literal guarantees wrapper name ==
operation name; unknown names fall through to the standard CoreWCF dispatch fault).

**Galileo client envelopes** are the outbound counterpart: byte-verified parity against captured
.NET Framework 4.8 proxy output (§3.7).

Legacy WSDL baselines used during the migration remain in the local, gitignored `baselines/` folder
(see [ado-retirement.md](ado-retirement.md)).

## 5. Migration tooling

Mechanics and usage live in [development-guide.md](development-guide.md); summary only:

- `tools/Generate-Contracts.ps1` — generates the CoreWCF contracts, MessageContracts, partial
  implementations, and `ServiceRoutes.g.cs` from the converted ASMX service classes, "preserving the
  ASMX wire format (wrapped doc/literal, {ns}/{method} SOAPAction, TripXML SoapHeader,
  {method}Result naming)" (script synopsis). Output goes to `src/wsTripXML/Code/Generated/`
  (105 `.g.cs` files: 104 service contracts + `ServiceRoutes.g.cs`).
- `tools/Convert-Services.ps1` — mechanically converts code-behinds: strips `System.Web.Services`
  attributes, replaces the designer region with a `modMain` DI constructor, swaps `Application`
  state for `TripXMLMain.AppState`, removes `System.Web` usings. Idempotent.
- `tools/ContractScan` (`tools/ContractScan/Program.cs`) — "replicates (approximately) the
  per-contract XmlSerializer import CoreWCF performs at host startup, so every XML type-name
  collision across all 104 service contracts is reported in ONE run instead of one boot-crash at a
  time."

## 6. Code review and remediation

A pre-commit multi-agent review surfaced a set of findings; the key remediations, each re-verified
in the committed working tree, are below:

| # | Finding | Remediation in committed code | Verified at |
|---|---|---|---|
| 1 | Default WCF message-size limit (64 KB) would reject real GDS payloads | `MaxReceivedMessageSize = 64_000_000` and `MaxBufferSize = 64_000_000` on every binding instance, HTTP and HTTPS | `src/wsTripXML/Program.cs` lines 51-56 (HTTP) and 63-68 (HTTPS) |
| 2 | Registering an HTTPS endpoint while Kestrel listens only on HTTP crashes the host | HTTPS endpoints are added only when the configured server URLs contain `https` (`urls` config / `ASPNETCORE_URLS`) | `src/wsTripXML/Program.cs` lines 34-38, 61-70 |
| 3 | Culture-sensitive VB conversions and XSLT extension functions break on non-en-US hosts | `CultureInfo.DefaultThreadCurrentCulture`/`DefaultThreadCurrentUICulture` pinned to `en-US` at startup ("Legacy servers ran en-US; XSLT extension functions and VB conversions depend on it") | `src/wsTripXML/Code/Classes/TripXMLRuntime.cs` lines 33-35 |
| 4 | Live credentials committed in config | `appsettings.json` is placeholders-only with an explicit header comment ("Real values come from environment variables (AppSettings__ConnectionString etc.), user-secrets, or an uncommitted appsettings.Production.json"); `Web.config`/`Web.Debug.config`/`Web.Release.config` deleted from the tree | `src/wsTripXML/appsettings.json`; commit `240561a` |
| 5 | `modMain` is stateful per-request glue; a singleton would cross-contaminate requests | Registered scoped: `builder.Services.AddScoped<modMain>();` services resolve it per call (`InstanceContextMode.PerCall` on every generated service) | `src/wsTripXML/Program.cs` line 18 |

### Sharp edge: Amadeus TLS validation defaults to OFF

The one remediation that is deliberately **legacy parity rather than a fix**. The legacy adapter
disabled TLS certificate validation process-wide; the migration scoped the bypass to the Amadeus
handler but kept bypass as the default. `src/Amadeus/ttHttpWebClient.cs` lines 63-71, verbatim:

```csharp
// The legacy adapter disabled TLS certificate validation process-wide via
// ServicePointManager.ServerCertificateValidationCallback. Preserve that default,
// scoped to the Amadeus handler only. Ops can re-enable validation by setting
// AmadeusSkipCertValidation=false in configuration.
string skipCertValidation = modCore.config["AmadeusSkipCertValidation"];
if (skipCertValidation == null || skipCertValidation.Equals("true", StringComparison.OrdinalIgnoreCase))
{
    handler.SslOptions.RemoteCertificateValidationCallback = (s, c, ch, e) => true;
}
```

Read the condition carefully: `skipCertValidation == null || ...Equals("true", ...)` means an
**unset** `AmadeusSkipCertValidation` key still bypasses TLS validation. Deleting the key does not
harden anything; the placeholder `appsettings.json` ships it as `"true"`. **Operations must
explicitly set `AppSettings__AmadeusSkipCertValidation=false`** (after confirming the Amadeus
endpoints present valid certificates) to enable certificate validation. Until then, Amadeus traffic
is vulnerable to man-in-the-middle interception exactly as it was on legacy.

## 7. Verification evidence (2026-06-12)

Environment: .NET SDK 10.0.300 (MSBuild 18.6.3+caa81fa49), Windows 11 Pro 10.0.26200, RID win-x64.

| Gate | Command | Result |
|---|---|---|
| Build | `dotnet build TripXML.slnx -c Release` | 0 errors, 839 warnings, 39.7 s |
| Tests | `dotnet test src/TripXMLMain/tests/TripXML.XsltTests -c Release` | 24/24 passed, 2 s |
| Host startup | process start → first HTTP 200 from `/wsPing.asmx?wsdl`, all 104 endpoints registered (25 ms poll granularity) | 1106 / 1113 / 1159 ms over 3 runs |
| SOAP round-trip | `wsPing` operation `ping` (`Delay=-1`, returns immediately), `Invoke-WebRequest` client over localhost loopback | first call 158.43 ms (includes XmlSerializer first-use codegen), second 11.39 ms, steady state over 20 calls: median 2.02 ms / min 1.68 / max 4.01 |

All runtime measurements are **new-stack only, on localhost loopback, Release build, placeholder
config — there is no legacy-side baseline to compare against.**

XSLT first-vs-cached cost via `CoreLib.TransformXML` (in-process harness, JIT pre-warmed, trivial
non-matching input `<root/>`):

| Stylesheet | Size | First call (compile) | Cached median |
|---|---|---|---|
| `v03_Sabre_PNRReadRS.xsl` | 254 KB | 78.28 ms | 0.016 ms |
| `Markups_LowFareRS.xsl` | 31 KB | 16.29 ms | 0.006 ms |
| `Aggregation_AirAvailRS.xsl` | 2 KB | 2.28 ms | 0.003 ms |

Caveat (mandatory): the cached medians measure cache-hit overhead plus a transform over trivial
input that matches no templates — they are **not** realistic payload transform costs. The valid
headline is that stylesheet compile cost (2-78 ms, scaling with size) is paid once per process per
stylesheet; the retired xsltc approach paid it at build time instead. Methodology and discussion:
[performance.md](performance.md).

## 8. Known gaps at migration time

Snapshot as of the migration commits; the **living list is maintained in
[development-guide.md](development-guide.md)**. Every marker below was re-verified in the working
tree on 2026-06-12.

- **`wsImportLog` is a stub.** `src/wsTripXML/Code/Admin/wsImportLog.asmx.cs` lines 28-31: the
  `ImportLog()` body only sends a trace — "cLog.ImportLog() was left commented out (half-refactored
  VB FileSystem code) by the VB->C# conversion; this admin endpoint is not deployed in production.
  Restore once cLog.ImportLog is finished."
- **SITA dispatch stubs (legacy parity).** `src/wsTripXML/Code/wsNative.asmx.cs` line 98
  (`case "sita":` with an empty body; the `SendOtherRequestSITA` call commented out at line 92) and
  `src/wsTripXML/Code/wsInventoryManagement.asmx.cs` line 99 (`case "SITA":` empty; call commented
  at line 97). These were already dead dispatch arms on legacy ([MIGRATION_STATUS.md](../MIGRATION_STATUS.md)
  §9 #4 — the SITA repo itself is WSE 3.0, archived, unbuildable); the migration preserved the stubs
  rather than resolving SITA's disposition.
- **ASPX pages not ported.** `GetFareRules.aspx.cs`/`.designer.cs` and
  `RulesResult.aspx.cs`/`.designer.cs` are excluded via `Compile Remove` in
  `src/wsTripXML/wsTripXML.csproj` (lines 38-41, marked "ASPX pages ported in Phase 10 (minimal API
  endpoints)"). Until that Phase-10 work lands, those two pages are not served.
- **Response compression not configured — capability regression.** Legacy applied the
  `CompressionExtension` ASMX `SoapExtension` per-method (gzip-in-SOAP). No `CompressionExtension`
  references remain in any source file under `src/wsTripXML` (the only remaining mention is the
  legacy ADO checkout line in `src/wsTripXML/azure-pipelines.yml`, line 18), and
  `src/wsTripXML/Program.cs` wires no `UseResponseCompression`/response-compression middleware.
  Clients that relied on compressed responses get uncompressed payloads until ASP.NET Core response
  compression is configured.
- **Conversion residue in decode paths.** In `src/wsTripXML/Code/wsInventoryManagement.asmx.cs` the
  legacy `ttAirports`/`ttAirlines` DataView lookups survive only as comments (lines 34-35, 47, 49,
  57); active decoding goes through `TripXMLLoad.DecodeValue`. Equivalent commented
  `GetEncodeValue(ttAirlinesNames, ...)` remnants exist in `Code/wsPNREnd.asmx.cs`,
  `Code/wsPNRRead_v03/_v04/_v05.asmx.cs`, and `Code/wsUpdateSessioned.asmx.cs`. Cosmetic, but it
  makes the decode path harder to read and should be cleaned up deliberately, not via find-and-delete.
- **Excluded dead code still tracked.** The 4-byte `wsAuthorization.asmx.cs`, fully commented-out
  `Code/Global.asax.cs`, and `My Project/**` are excluded from compilation
  (`src/wsTripXML/wsTripXML.csproj` lines 33-46) but remain in the tree.

## 9. Related documents

| Document | Content |
|---|---|
| [docs/README.md](README.md) | Documentation index |
| [docs/architecture.md](architecture.md) | System architecture after the migration |
| [docs/performance.md](performance.md) | Measurement methodology, full performance data |
| [docs/development-guide.md](development-guide.md) | Build/test/tooling workflows; living gap list |
| [ADR-0001](adr/0001-corewcf-over-rest-rewrite.md) | CoreWCF over a REST rewrite |
| [ADR-0002](adr/0002-runtime-xslt-compilation.md) | Runtime XSLT compilation over xsltc |
| [ADR-0003](adr/0003-handrolled-soap-clients.md) | Hand-rolled SOAP clients over generated proxies |
| [ADR-0004](adr/0004-appstate-facade.md) | AppState facade over IMemoryCache |
| [ADR-0005](adr/0005-modcore-config-bridge.md) | modCore.config bridge |
| [ADR-0006](adr/0006-monorepo-with-history.md) | Monorepo with imported history |
| [docs/monorepo-consolidation-design.md](monorepo-consolidation-design.md) | Monorepo assembly plan (pre-existing) |
| [docs/ado-retirement.md](ado-retirement.md) | ADO retirement checklist (pre-existing) |
| [MIGRATION_STATUS.md](../MIGRATION_STATUS.md) | 2026-06-10 pre-migration audit of the 17 ADO repos |
