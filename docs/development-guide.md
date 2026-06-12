# TripXML development guide

How to build, run, test, and safely change the TripXML service host after the .NET 10 migration
(executed 2026-06-11, landed in nine commits — `9b8f052` (TripXMLMain) first through `4cda4b7`
(XSLs) last; `git log --oneline 9b8f052^..4cda4b7`). Written for both human developers
and AI agents: every workflow names the exact files, symbols, and commands involved. For *why*
the system is shaped this way, read [architecture.md](architecture.md) and the
[ADRs](adr/README.md); for *what changed* in the migration, read
[migration-report.md](migration-report.md).

## 1. Prerequisites and quickstart

- **.NET SDK 10** — verified against SDK 10.0.300 (MSBuild 18.6.3) on Windows 11. The repo has
  no `global.json`; any 10.x SDK should work.
- Windows is the only verified platform. The code base has Windows-era assumptions
  (`Microsoft.VisualBasic`, backslash path literals in `appsettings.json` defaults).

```powershell
# Build everything (9 projects in TripXML.slnx)
dotnet build TripXML.slnx -c Release
# Verified 2026-06-12: 0 errors, 839 warnings (pre-existing analyzer findings, e.g. CA2200), 39.7s

# Run the test suite
dotnet test src/TripXMLMain/tests/TripXML.XsltTests -c Release
# Verified 2026-06-12: 24/24 passed in 2s

# Run the host (uses src/wsTripXML/Properties/launchSettings.json:
# https://localhost:63071 and http://localhost:63072)
dotnet run --project src/wsTripXML

# Or run the built assembly with explicit URLs
dotnet src/wsTripXML/bin/Release/net10.0/wsTripXML.dll --urls "http://localhost:63072"

# Smoke test
curl http://localhost:63072/wsPing.asmx?wsdl
```

Notes that save agents time:

- **Placeholder config boots fine.** The committed `src/wsTripXML/appsettings.json` contains
  placeholders only. Startup table loading (`modMain.TripXMLStartUp()` +
  `TripXMLTools.TripXMLLoad.BuildDecodingDataViews()`) runs as a background `Task.Run` registered
  on `ApplicationStarted` and catches its own failures
  (src/wsTripXML/Code/Classes/TripXMLRuntime.cs:57-72), so the host comes up even when SQL and
  Hasura are unreachable. Measured cold start (process start to first HTTP 200 from
  `/wsPing.asmx?wsdl`, all 104 CoreWCF endpoints registered; new stack only — there is no
  legacy-side baseline): 1106 / 1113 / 1159 ms over 3 runs. See [performance.md](performance.md).
- **HTTPS endpoints are conditional.** `src/wsTripXML/Program.cs:35-38` registers the
  Transport-secured `BasicHttpBinding` endpoints only when the configured `urls` /
  `ASPNETCORE_URLS` contain `https`. Running with an http-only `--urls` is valid.
- The first SOAP call to a service is slow by design: `XmlSerializer` generates serialization
  code on first use (measured on `wsPing` `ping`: first call 158.43 ms, second 11.39 ms,
  steady-state median 2.02 ms over 20 calls — localhost loopback, no legacy baseline).

## 2. Repository map

| Path | What it is |
|---|---|
| `TripXML.slnx` | Solution: 7 libraries + host + test project. `tools/ContractScan` is *not* in the solution. |
| `src/wsTripXML/` | The CoreWCF host (`Microsoft.NET.Sdk.Web`, exe). 104 SOAP services: `Program.cs`, `Code/` code-behinds, `Code/Generated/` contracts, `.asmx` marker files at the root, `App_Data/Admin/` markers for the two `/Admin/*` services. |
| `src/wsTripXML/Code/Generated/` | 105 generated `.g.cs` files: 104 service contracts + `ServiceRoutes.g.cs` (`ServiceRoutes.All`, exactly 104 entries). Never hand-edit (see section 3). |
| `src/TripXMLMain/` | The hub library every other project references: `CoreLib.cs` (runtime XSLT engine + `SendTrace`), `modCore.cs` (config bridge, shared structs), `AppState.cs` (`HttpApplicationState` replacement, [ADR-0004](adr/0004-appstate-facade.md)), `cDA.cs` (SQL access), `TtVbXsltFunctions.cs` (`urn:ttVB` XSLT extension object). |
| `src/TripXMLMain/tests/TripXML.XsltTests/` | The xUnit test project (nested here because its git history lives with TripXMLMain). |
| `src/Amadeus/`, `src/Galileo/`, `src/Sabre/`, `src/TravelPort/`, `src/Worldspan/` | GDS adapters with hand-rolled SOAP clients ([ADR-0003](adr/0003-handrolled-soap-clients.md)). The Amadeus project file is `AmadeusWS.csproj`. |
| `src/TripXMLTools/` | Hasura-backed loading of credentials, provider settings, and decoding tables (`TripXMLLoad.cs`, `SettingsService.cs`, DTOs). |
| `src/XSLs/` | 553 `.xsl` stylesheets (data, no project file). In-scope folders ship with the host via the `Content` glob in `src/wsTripXML/wsTripXML.csproj`. |
| `tools/` | `Generate-Contracts.ps1`, `Convert-Services.ps1`, `ContractScan/` — the contract pipeline (section 3). |
| `docs/` | This documentation set; index in [README.md](README.md). |
| `baselines/` | **Gitignored.** Local captures: legacy WSDL snapshots, gitleaks reports, and the golden-transform corpus under `baselines/xslt/` (section 4.4). Never commit. |
| `repos/` | **Gitignored.** Retired per-repo ADO clones; deletion tracked in [ado-retirement.md](ado-retirement.md). |
| `.gitnexus/` | **Gitignored.** GitNexus code-intelligence index used by the agent tooling in `CLAUDE.md`. Reindexed 2026-06-12 over the full monorepo (1,369 files, 17,318 symbols, 281 execution flows); before that it covered only 6 root-level files. Re-run `node .gitnexus/run.cjs analyze` after large refactors so the `impact()`/`detect_changes()` rules in `CLAUDE.md` stay meaningful. |
| `MIGRATION_STATUS.md` | Historical pre-migration audit (2026-06-10), preserved as the "before" record. |

## 3. Contract regeneration pipeline

The ASMX-to-CoreWCF wire format ([ADR-0001](adr/0001-corewcf-over-rest-rewrite.md)) is produced
by generated code, not hand-written code. Three tools own it:

| Tool | Role |
|---|---|
| `tools/Generate-Contracts.ps1` | Emits `Code/Generated/{cls}.g.cs` (contract interface, MessageContracts, partial bridge class) and `Code/Generated/ServiceRoutes.g.cs` from ASMX-style code-behinds. |
| `tools/ContractScan/` | Pre-flight `XmlSerializer` collision check over all 104 contracts. |
| `tools/Convert-Services.ps1` | One-shot historical converter of ASMX code-behinds to CoreWCF partial classes. Idempotent. |

**Rule: never hand-edit `Code/Generated/*.g.cs`.** Every file carries a
`// Generated by tools/Generate-Contracts.ps1 - do not edit by hand.` banner. Hand edits are
silently lost on the next generator run and break the guarantee that the wire format is derived,
not curated.

### When to regenerate

- Adding a new service (recipe 4.1).
- Changing an operation signature — name, parameter, return type, or `SoapHeader` usage —
  on an existing service (recipe 4.2).
- Renaming a service or changing its `[WebService]` `Namespace`/`Name`.

Body-type-only changes (fields inside an existing request/response class) do **not** require
regeneration — the generated contracts reference the code-behind types by name — but they *do*
require a ContractScan run and a wire-parity review (recipe 4.2).

### Generate-Contracts.ps1 — actual behavior

```powershell
pwsh tools/Generate-Contracts.ps1 [-HostRoot <path>] [-Only wsFoo,wsBar]
```

- `-HostRoot` defaults to `..\src\wsTripXML` relative to the script (tools/Generate-Contracts.ps1:15).
  You only pass it when operating on a copy of the host tree.
- The script discovers routes by scanning `*.asmx` marker files at the host root and recursively
  under `App_Data/` for their `Class="..."` attribute; markers under `App_Data\Admin\` map to
  `/Admin/{name}.asmx` routes (lines 25-37).
- For each service it parses `[WebMethod]` operations out of the code-behind with regexes
  (lines 67-85). Constraints an agent must know: **at most one parameter per operation** is
  supported by the parser, and a `[SoapHeader("field")]` attribute switches the operation to
  MessageContract mode (TripXML SOAP header + `{method}Result`-named response member).
- If a code-behind has **no** `[WebMethod]` left (i.e. it was already converted), the service's
  contract is *not* regenerated; its route is kept only if `Code/Generated/{cls}.g.cs` already
  exists (lines 86-96). This is the normal steady state for all 104 services today — `grep`
  confirms zero `[WebMethod]` attributes remain under `src/wsTripXML`.

> **Warning — `-Only` truncates `ServiceRoutes.g.cs`.** The route table is rewritten
> unconditionally at the end of every run (lines 182-200), but route entries are only collected
> for services that pass the `-Only` filter (line 57). A run with `-Only wsFoo` therefore leaves
> `ServiceRoutes.All` with a single entry and the host serving one endpoint. `-Only` is fine as
> an inner-loop shortcut, but **always finish with a full run (no `-Only`)** to rebuild the
> complete 104-entry table before building, committing, or running ContractScan.

### ContractScan — pre-flight collision check

```powershell
dotnet run --project tools/ContractScan -c Release
# exit code 0 = clean; 1 = failures, one line per contract/type collision
```

CoreWCF performs a per-contract `XmlSerializer` import at host startup and crashes on the
*first* XML type-name collision it finds. `tools/ContractScan/Program.cs` replicates that import
over every contract in `wsTripXML.Hosting.ServiceRoutes.All` so all collisions across all 104
contracts are reported in one run instead of one boot-crash at a time. Run it after any contract
regeneration or any change to a serialized request/response type, before booting the host.

### Convert-Services.ps1 — historical, idempotent

```powershell
pwsh tools/Convert-Services.ps1 [-HostRoot <path>] [-Files wsFoo.asmx.cs]
```

This performed the one-shot mechanical conversion of all code-behinds: strips
`System.Web.Services` attributes (including `[CompressionExtension.CompressionExtension()]`,
`[ScriptService]`, `[ScriptMethod]`), replaces the designer region with a `modMain` DI
constructor, rewrites `Application.*` state access to `TripXMLMain.AppState.*`, and removes
`System.Web` usings. It is idempotent (a second run produces no changes), so it is safe — and
required — as the final "strip the temporary attributes again" step in recipes 4.1 and 4.2.

## 4. Recipes

### 4.1 Add a new SOAP endpoint

1. Write the code-behind at `src/wsTripXML/Code/wsFoo.asmx.cs` in **ASMX style** so the generator
   can parse it: namespace `wsTripXML.wsTravelTalk`, a
   `[WebService(Namespace = "...", Name = "wsFoo")]` class with `[WebMethod]` operations, a
   public `TripXML tXML;` field, and `[SoapHeader("tXML")]` on operations that take the TripXML
   header. Keep each operation to **one parameter** (generator limit, section 3).
2. Add the marker file `src/wsTripXML/wsFoo.asmx` (or `src/wsTripXML/App_Data/Admin/wsfoo.asmx`
   for an `/Admin/*` route). Only the `Class` attribute matters; follow the existing pattern,
   e.g. `src/wsTripXML/wsPing.asmx`:
   `<%@ WebService Language="vb" CodeBehind="~/Code/wsPing.asmx.vb" Class="wsTripXML.wsTravelTalk.wsPing" %>`.
3. Generate: `pwsh tools/Generate-Contracts.ps1` (full run — it regenerates `wsFoo.g.cs` from the
   `[WebMethod]`s and keeps all existing routes via their `.g.cs` presence).
4. Strip the temporary ASMX attributes and inject the DI constructor:
   `pwsh tools/Convert-Services.ps1 -Files wsFoo.asmx.cs`.
5. Pre-flight: `dotnet run --project tools/ContractScan` must exit 0.
6. Build and verify: `dotnet build TripXML.slnx`, run the host, fetch `/wsFoo.asmx?wsdl`, and
   POST a SOAP request. No `Program.cs` edit is needed — it registers every `ServiceRoutes.All`
   entry in DI and as a CoreWCF endpoint automatically (src/wsTripXML/Program.cs:25-28, 42-72).

### 4.2 Modify an existing endpoint without breaking wire parity

The converted code-behinds no longer carry `[WebMethod]`, so the generator cannot re-derive a
contract from them as-is.

1. Run impact analysis first (the `CLAUDE.md` GitNexus rules require it for any symbol edit).
2. Temporarily re-add the ASMX attributes on the changed class/methods — `[WebService(...)]` with
   the service's original `Namespace`/`Name` (copy them from the header comment of the existing
   `Code/Generated/{cls}.g.cs`), `[WebMethod]`, and `[SoapHeader("tXML")]` where the operation
   takes the header.
3. `pwsh tools/Generate-Contracts.ps1` (full run, not `-Only` — see the truncation warning).
4. `pwsh tools/Convert-Services.ps1 -Files {cls}.asmx.cs` to strip the attributes again.
5. `dotnet run --project tools/ContractScan` must exit 0.
6. Walk the wire-parity invariants checklist in
   [architecture.md](architecture.md) (SOAPAction format, wrapper element names,
   `{method}Result` response member, TripXML header namespace, `RequestElementRoutingBehavior` /
   `SoapRequestInspectorBehavior` expectations) and diff the served WSDL against the legacy
   capture in `baselines/` if you have one locally.
7. Anything other than an additive, optional change to a request/response type is a breaking
   wire change for deployed SOAP clients. Name it as such in the commit message.

### 4.3 Touch a GDS adapter

Shared conventions (verify against the file you are editing; all are deliberate legacy-parity
decisions documented in [ADR-0003](adr/0003-handrolled-soap-clients.md)):

- **One static pooled `HttpClient` per adapter** over `SocketsHttpHandler` with
  `PooledConnectionLifetime = TimeSpan.FromMinutes(5)` (keeps DNS fresh) and automatic
  decompression: `src/Sabre/ttHttpWebClient.cs:27`, `src/TravelPort/ttHttpWebClient.cs:14`,
  `src/Worldspan/ttHttpWebClient.cs:16`, `src/Galileo/Classes/GalileoSoapClient.cs:54`,
  `src/Amadeus/ttHttpWebClient.cs:33` (a `Lazy<HttpClient>`, so `modCore.config` is populated
  before the certificate policy is read). `src/Amadeus/BlackListClient.cs:24` follows the same
  pooled-handler pattern but configures no decompression (its responses are small XML). Do not
  reintroduce per-call clients or `HttpWebRequest`.
- **Timeout parity with the legacy adapters is intentional.** Amadeus uses
  `HttpClient.Timeout` = 90 s, mirroring legacy `mHttpRequest.Timeout = 90000`
  (src/Amadeus/ttHttpWebClient.cs:73-74). Sabre enforces 60 s per request via a
  `CancellationTokenSource` (`REQUEST_TIMEOUT_MS`, src/Sabre/ttHttpWebClient.cs:22). Changing a
  timeout is a behavior change for production traffic — treat it like a contract change.
- Adapters read configuration through `TripXMLMain.modCore.config` and shared state through
  `TripXMLMain.AppState` — never reference the host project from an adapter.
- The Amadeus TLS-validation bypass (`AmadeusSkipCertValidation`) is described in section 5.

### 4.4 Add or modify an XSLT stylesheet

- Stylesheets live under `src/XSLs/{folder}/`. The host ships these folders to
  `{output}/Xsl/...` via the `Content` glob in `src/wsTripXML/wsTripXML.csproj` (Amadeus,
  AmadeusWS, Galileo, Sabre, Worldspan, Travelport, Aggregation, BL, TXML — `LinkBase="Xsl"`,
  `CopyToOutputDirectory="PreserveNewest"`). A stylesheet in a *new* top-level folder is not
  shipped until you extend that glob.
- Engine settings are fixed: `document()` enabled, `msxsl:script` **disabled**. All script
  blocks were removed in commit `4cda4b7` (27 stylesheets, 28 files changed); their VB functions
  now live in the `urn:ttVB` extension object `TripXMLMain.TtVbXsltFunctions` (see
  [ADR-0002](adr/0002-runtime-xslt-compilation.md)). A reintroduced `msxsl:script` block fails
  `XsltCompileAllTests`.
- Compilation happens at runtime, once per process per stylesheet, cached in
  `CoreLib._xslCache` (src/TripXMLMain/CoreLib.cs:25-28). Measured compile cost scales with
  stylesheet size — 2.28 ms (2 KB) to 78.28 ms (254 KB) first call, in-process harness, no
  legacy baseline; the retired xsltc approach paid this at build time instead. Details and
  caveats in [performance.md](performance.md).
- **Tests that gate every stylesheet change** (`dotnet test src/TripXMLMain/tests/TripXML.XsltTests`):
  - `XsltCompileAllTests` loads every `.xsl` in the nine in-scope folders with the same engine
    settings `CoreLib.TransformXML` uses. Portal*, SITA, and TravelFusion folders are explicitly
    out of scope (archived components).
  - `GoldenTransformTests` replays golden cases through `CoreLib.TransformXML`. Each case is a
    directory `baselines/xslt/{GDS}/{case}/` containing `input.xml`, `stylesheet.txt` (relative
    stylesheet path within the XSLs tree), and `expected.xml` captured from the legacy system.
    Path resolution (`src/TripXMLMain/tests/TripXML.XsltTests/XslTestPaths.cs`): the XSLs root is
    found by walking up from the test bin folder, overridable via `XSLS_ROOT`; the baselines root
    resolves `baselines/xslt` the same way, overridable via `XSLT_BASELINES_ROOT`. Because
    `baselines/` is gitignored, **the suite passes with zero cases when the folder is absent or
    empty** — it reports the discovered count in the assertion message so silent emptiness is
    visible (`GoldenTransformTests.cs:66-69`). If you have legacy captures, populate cases
    locally; they are picked up automatically.

## 5. Configuration and secrets

### Placeholders-only policy

`src/wsTripXML/appsettings.json` is committed with **placeholders only** (the policy is stated in
the file itself, lines 10-12). Real values come from environment variables, user-secrets, or an
uncommitted `appsettings.Production.json`. Never commit live values — including in documentation
and commit messages. `env.txt` (gitignored) holds local credentials and must never enter history.

### How configuration flows

`TripXMLRuntime.Initialize` (src/wsTripXML/Code/Classes/TripXMLRuntime.cs:38-52) copies the
`AppSettings` configuration section into `TripXMLMain.modCore.config` (a `NameValueCollection`,
the replacement for `ConfigurationManager.AppSettings`) once at startup, then derives
`modCore.XslPath` / `modCore.LogPath` / `modCore.SchemaPath` from `XslPath` /
`TripXMLLogFolder` / `SchemaPath`, falling back to `{TripXMLFolder or ContentRoot}\Xsl|Log|Schemas`.
Lookups of missing keys return `null` (standard `NameValueCollection` semantics); callers carry
their own defaults. See [ADR-0005](adr/0005-modcore-config-bridge.md).

Because the section name is `AppSettings`, the standard ASP.NET Core environment-variable mapping
applies: `AppSettings__ConnectionString`, `AppSettings__HasuraEndpoint`, etc. One host-level key
lives *outside* that section: `IncludeExceptionDetailInFaults` (default `false`,
src/wsTripXML/Program.cs:40) controls whether CoreWCF faults carry exception detail — keep it
`false` anywhere a client can see faults.

### Local development

```powershell
# Option A: environment variables (works everywhere)
$env:AppSettings__ConnectionString = "..."; dotnet run --project src/wsTripXML

# Option B: user-secrets (Development environment only; no UserSecretsId is committed yet,
# so init once — it adds the property to wsTripXML.csproj, which is fine to commit)
dotnet user-secrets init --project src/wsTripXML
dotnet user-secrets set "AppSettings:ConnectionString" "..." --project src/wsTripXML
```

### Secrets hygiene

`.gitleaks.toml` extends the default gitleaks rules with one allowlist: `CoreLib.SendTrace`
trace-banner string literals whose first argument is `ttCredential.UserID` trip the
generic-api-key rule; the quoted strings are log banners, not secrets. Run
`gitleaks detect` before pushing anything that touched configuration or history. History was
already scrubbed with git-filter-repo on 2026-06-12 (see
[monorepo-consolidation-design.md](monorepo-consolidation-design.md)); credential rotation is
tracked in [ado-retirement.md](ado-retirement.md).

### Deployment note: AmadeusSkipCertValidation

The legacy adapter disabled TLS certificate validation process-wide; the port preserves that
default **scoped to the Amadeus handler**: when `AmadeusSkipCertValidation` is **unset or
"true"**, `RemoteCertificateValidationCallback` accepts every certificate
(src/Amadeus/ttHttpWebClient.cs:67-71). The committed placeholder sets it to `"true"`. In any
hardened environment, set `AppSettings__AmadeusSkipCertValidation=false` **explicitly** — leaving
it unset still bypasses validation. Background in
[migration-report.md](migration-report.md).

## 6. Testing

The suite is `src/TripXMLMain/tests/TripXML.XsltTests` (xUnit 2.9, net10.0). 24 tests, all
passing as of 2026-06-12:

| Test class | Tests | What it gates |
|---|---|---|
| `TtVbXsltFunctionsTests` | 14 | Value tables for the `urn:ttVB` extension object that replaced the `msxsl:script` VB blocks — `FctDateDuration` (7 cases across 2 theories), `FctArrDate` (2 facts), `ShortDateFormat` (3 cases), `GetBirthDate`, `datenow`. Culture pinned to en-US to match legacy servers. |
| `XsltCompileAllTests` | 9 | One theory case per in-scope stylesheet folder: every `.xsl` must load with the production engine settings (`document()` on, script off). The main migration gate for stylesheet changes. |
| `GoldenTransformTests` | 1 | One aggregating fact that replays every `baselines/xslt` golden case through `CoreLib.TransformXML` and diffs against legacy-captured output. Passes with zero cases when the gitignored corpus is empty (section 4.4). |

**What is *not* covered** — the honest list, and the roadmap for future test work:

- No endpoint integration tests: nothing boots the host and POSTs SOAP to the 104 endpoints.
  (The wsPing measurements in [performance.md](performance.md) were ad-hoc, not a committed test.)
- No GDS adapter tests: request composition, response handling, and timeout behavior of the
  five adapters are untested.
- No host-level tests: routing, `RequestElementRoutingBehavior`, `SoapRequestInspectorBehavior`,
  and WSDL emission have no automated checks; `tools/ContractScan` is the only startup gate.
- The golden-transform corpus is empty until `baselines/xslt` is populated from legacy captures.

## 7. Known gaps and roadmap

Canonical living list — [migration-report.md](migration-report.md) links here instead of
duplicating it. Each entry is verified in code; re-verify before working on one.

| # | Gap | Evidence | Status / next step |
|---|---|---|---|
| 1 | `wsImportLog` is a stub | `ImportLog()` only emits a SendTrace "not available in this build" because `cLog.ImportLog()` was left commented out half-refactored (src/wsTripXML/Code/Admin/wsImportLog.asmx.cs:24-36; src/wsTripXML/Code/Classes/cLog.cs:162). | Finish porting `cLog.ImportLog`, then restore the call. |
| 2 | SITA dispatch is a no-op | Empty `{ break; }` blocks return nothing in both dispatchers: `case "sita":` (src/wsTripXML/Code/wsNative.asmx.cs:98-101) and `case "SITA":` (src/wsTripXML/Code/wsInventoryManagement.asmx.cs:99); the legacy SITA repo is archived (WSE 3.0, dead tech). | Decide: remove the case and fail loudly, or port a SITA client. Currently a SITA request falls through silently. |
| 3 | `GetFareRules.aspx` / `RulesResult.aspx` not ported | Both are `Compile Remove`d in src/wsTripXML/wsTripXML.csproj:37-41. The csproj comment anticipates a Phase 10 minimal-API port, but no such endpoints exist in `Program.cs` — the pages 404 today. | Port as minimal API endpoints or declare the pages retired. |
| 4 | No response compression | Legacy ASMX compressed responses via the `CompressionExtension` SoapExtension (its attribute is one of the patterns Convert-Services.ps1 strips, tools/Convert-Services.ps1:31). The CoreWCF host configures no response compression — zero matches for `Compression` in src/wsTripXML `*.cs`. | Add ASP.NET Core response compression if clients depended on it; verify client Accept-Encoding behavior first. |
| 5 | `ttAirlinesNames` never populated | `BuildDecodingDataViews` seeds only `ttAirports`, `ttAirlines`, `ttEquipments` (src/TripXMLTools/TripXMLLoad.cs:104-106). `wsLowFarePlus_v03.asmx.cs:58` still reads `AppState.Get("ttAirlinesNames")` and gets `null`; `modMain.GetDecodeValue` returns `string.Empty` on a null view (src/wsTripXML/Code/modMain.cs:957), so the airline name-to-code reverse decode is silently disabled. All other references are commented out. | Either add an AirlinesNames view to `BuildDecodingDataViews` or remove the dead read path. |
| 6 | No JSON/AJAX shim | The legacy `[ScriptService]`/`[ScriptMethod]` JSON POST surface was stripped without replacement (patterns at tools/Convert-Services.ps1:34-35); only commented-out boilerplate remains (e.g. src/wsTripXML/Code/wsAdmin.asmx.cs:12-13). The host serves SOAP 1.1 only. | Confirm no production client used the JSON surface; add a minimal-API shim only if one did. |
| 7 | Stale `wsAuthorization` duplicate at the host root | The service itself **is** hosted: src/wsTripXML/Code/wsAuthorization.asmx.cs implements it, Code/Generated/wsAuthorization.g.cs defines the contract, and ServiceRoutes.g.cs:25 registers `/wsAuthorization.asmx` (one of the 104). The anomaly is an empty (4-byte) duplicate code-behind at the host root (src/wsTripXML/wsAuthorization.asmx.cs), excluded via `Compile Remove`, plus a misleading csproj comment claiming "no service to host" (src/wsTripXML/wsTripXML.csproj:34-35). | Delete the empty root-level file and fix the csproj comment. |
| 8 | Golden-transform corpus empty | Section 4.4 / section 6. | Capture legacy transform outputs into `baselines/xslt` while a legacy environment still exists. |
| 9 | Galileo legacy `ttHttpWebClient` not rewritten | src/Galileo/ttHttpWebClient.cs still runs on `HttpWebRequest` with the `_HttpResponse` reflection fallback (line 81); no in-tree callers — live Galileo traffic goes through `GalileoSoapClient`. Recorded as the known residual in [ADR-0003](adr/0003-handrolled-soap-clients.md). | Rewrite on the shared `HttpClient` pattern or delete the dead class. |

## 8. Documentation maintenance

- **[architecture.md](architecture.md)** must be updated whenever you change `Program.cs`
  bindings/behaviors, the generated-contract shape, routing
  (`RequestElementRoutingBehavior`), the startup sequence (`TripXMLRuntime`), or the wire-parity
  invariants its checklist documents.
- **ADRs**: significant decisions get `docs/adr/NNNN-kebab-title.md` in the existing MADR-lite
  shape (Context / Decision / Consequences), numbered sequentially after
  [0006](adr/0006-monorepo-with-history.md), plus a row in [adr/README.md](adr/README.md).
  Supersede, don't rewrite: a reversed decision gets a new ADR that links the old one.
- **`CLAUDE.md` and `AGENTS.md` must stay byte-identical** (verified identical today). Edit one,
  copy to the other in the same commit: `Copy-Item CLAUDE.md AGENTS.md`.
- **This guide's section 7** is the canonical gap list — update it (with file:line evidence) when
  closing or discovering a gap, rather than scattering TODOs.
- **[README.md](README.md)** (the docs index) gets a row for every new document.
- Keep numbers honest: cite only what you measured in this repo or can point to in code, label
  measurement conditions (the performance numbers in this doc set are new-stack-only, localhost,
  with no legacy baseline), and never paste secrets — `appsettings.json` stays placeholders-only
  even inside quoted examples.
