# TripXML .NET Migration Status

Reviewed: 2026-06-10 (automated multi-agent audit of all 17 ADO repos + branches)

---

## 1. TL;DR

- **Almost nothing is migrated.** Of 17 repos, exactly one (TripXMLLibrary) has an SDK-style project with a modern TFM (net472;net6.0) on master — and its net6.0 target is EOL and its core runtime path (loading .NET Framework xsltc-compiled XSL DLLs) is unverified and likely broken.
- **The real migration work lives on one unmerged branch**: `wsTripXML@converted` — a genuine, complete VB→C# translation (352 .vb → 354 .cs, 0 VB left) plus an SDK-style net8.0 csproj. But it **cannot compile**: no entry point, 100+ code-behinds still on `System.Web.Services` (does not exist on .NET 8), all 7 sibling ProjectReferences still legacy net48. It trails master by 24 commits (~4 months) and is not merged.
- **The system's true blockers are not in ADO at all.** Modern multi-targeted (net48+net6.0) sources for `TripXML.Core` and `PaymentServices` exist somewhere outside the TTC org (verified org-wide absent); only their packages on the private feed `https://techtransnuget.azurewebsites.net` are visible. `TripXML.sln` references them via a path into a `technologytransferusa` folder that exists on no machine but the author's.
- **The dependency hub is TripXMLMain** (legacy net48, VB→C# already done): 6 repos ProjectReference it, and it in turn depends on ~285 precompiled .NET-Framework-only XSLT assemblies (xsltc output, some with VB script blocks) — the single hardest technical problem in the migration.
- **Production secrets are everywhere**: hardcoded production GDS credentials in source (AdminTools.cs, umbrella's XMLFile2.xml and a Postman collection), ~208 GDS credential pairs in wsTripXML's `Tables.zip` (also uncompressed in history, 34 blob versions), SQL `sa`/app connection strings, an Octopus API key, and a NuGet feed API key that even ships *inside* the published TripXML.Library nupkg. **History rewrite + credential rotation is mandatory before any GitHub push.**
- **The umbrella repo is unclonable**: 15 gitlinks with no `.gitmodules` (never existed in history) — a fresh clone yields empty directories — plus one dead gitlink (`VirtualCreditCard/VirtualCreditCard` @ c2b91ed9; repo deleted from all 19 org projects / 36 repos).
- **CI is essentially absent**: the ADO TripXML project has exactly one build pipeline (`wsTripXML`, YAML, id 22, created 2023-08-18). The other 16 repos have no CI.
- **Four repos are archive candidates, not migration candidates**: Portal (dead code system-wide), SITA (dead WSE 3.0 tech, unbuildable as cloned), TripXMLMain_VB (superseded by the C# conversion), AdminTools (dormant since 2022-11, consumer-only).
- Active team: Andrii (Andrew) Samokhvalov, Alexander Kobelev, Anton Ryasnenko; a "softraider" account performed the Jan 2026 VB→C# conversion commits.

---

## 2. Migration matrix

| Repo | Last active | Language | Project format | Target FW | Pkg mgmt | Status | Key blocker |
|---|---|---|---|---|---|---|---|
| AdminTools | 2022-11 | C# | legacy | net472 | none (GAC) | not-started | Config-based WCF clients; **prod GDS creds hardcoded** |
| Amadeus | 2026-05-14 | C# | legacy | net48 | PackageReference (3) | not-started | `HttpContext.Current` host coupling; ASMX client proxy |
| CompressionExtension | 2022-11 | C# | legacy | net48 | PackageReference (1) | not-started | Is an ASMX `SoapExtension` — no modern equivalent |
| Galileo | 2025-12-03 | C# | legacy | net48 (+orphan net40) | PackageReference (2) | not-started | 4 ASMX Web Reference proxies (`SoapHttpClientProtocol`) |
| PaymentServices | 2022-07 | C# | legacy | net472 | packages.config (23) | not-started* | ADO clone is stale; modern sources live outside ADO |
| Portal | 2014-12 (TFVC import) | VB.NET | legacy | net40 | none | not-started → **archive** | Dead code; ProjectReference to deleted vbproj |
| Sabre | 2026-02-03 | C# | legacy | net48 | none | not-started | TripXMLMain ref; reflection on FW-internal `_HttpResponse` |
| SITA | 2014-04 (TFVC import) | C# | legacy | net40 | none | not-started → **archive?** | WSE 3.0 `SoapClient` (dead tech); WSE DLL absent, unbuildable |
| TravelPort | 2024-08 | C# | legacy | net48 | none | not-started | TripXMLMain ref only; mechanically easy otherwise |
| TripXML (umbrella) | 2026-05-15 | sln + C# tests | legacy + sln | net48 | PackageReference (MSTest) | not-started | 15 gitlinks, no `.gitmodules`; 2 sln paths outside repo; secrets |
| TripXMLLibrary | 2022-04 | C# | **SDK** | **net472;net6.0** | PackageReference (7) | **in-progress** | net6 target loads FW xsltc DLLs at runtime (unverified); no known consumer |
| TripXMLMain | 2026-05-14 | C# (ex-VB) | legacy | net48 | PackageReference (3) | not-started | ~285 refs to precompiled FW-only XSLT assemblies |
| TripXMLMain_VB | 2022-02 | VB.NET | legacy | net472 | PackageReference (1) | n/a → **archive** | Superseded by TripXMLMain C# conversion |
| TripXMLTools | 2026-04-02 | C# | legacy | net48 | PackageReference (8) | not-started | `WebConfigurationManager` (System.Web); TripXMLMain ref |
| Worldspan | 2025-12-03 | C# | legacy | net48 | PackageReference (1) | not-started | TripXMLMain ref; reflection on FW-internal `_HttpResponse` |
| wsTripXML (master) | 2026-05-21 | VB.NET | legacy web app | net48 | packages.config (41) | in-progress (via branch) | 104 ASMX endpoints on System.Web; **secrets in Web.config + Tables.zip** |
| **wsTripXML@converted** | 2026-05-21 (df827f5) | C# | **SDK (Sdk.Web)** | **net8.0** | PackageReference | **in-progress** | No entry point; System.Web.Services code; 7 net48 sibling refs |
| XSLs | 2026-05-19 | XSLT (553 files) | n/a (no projects) | n/a | n/a (NuGet producer) | n/a (data repo) | xsltc.exe toolchain FW-only; msxsl:script VB blocks (18 in live build path) |

\* PaymentServices: the cloned repo is legacy net472 with a *reverted* modernization attempt (commit cbcdb30, 2022-07), yet the feed's `PaymentServices 1.0.34` package multi-targets net48+net6.0 — built from sources not in this org.

---

## 3. The wsTripXML .NET 8 conversion — deep dive

Branch topology: merge-base `8bdf751`; `converted` has 8 unique commits, master has 24 unique (Jan 13 – May 19, 2026). **Not merged** (verified by ancestry). History: `e76a45f` "convereted to c#" + `6d7f2ff` "renived vb-files" (2026-01-16, softraider; +405k C# lines, −369k VB), five "upd" commits (Samokhvalov, May 7–21), final `df827f5` "converted to net 8" (2026-05-21) — which touched only 2 files (+128/−167).

**What is real:**
- VB→C# translation is complete: master 352 .vb / 8 .cs → converted 0 .vb / 354 .cs, ~1:1 file mapping. Converter output (pervasive `Microsoft.VisualBasic.CompilerServices` usings) but genuine.
- `wsTripXML-converted/wsTripXML.csproj` is real SDK work: `Microsoft.NET.Sdk.Web`, net8.0, OutputType Exe, CoreWCF.Http 1.9.0, Microsoft.Extensions.* 10.0.8 (10.x on net8.0 — version-mismatch risk), TripXML.Core 1.0.19 + PaymentServices 1.0.34 from the private feed.
- Some plumbing genuinely modernized: `Code/modMain.cs` uses IHttpContextAccessor/IMemoryCache/IConfiguration; `Code/Classes/cSoapRQ.cs` ports the SOAP-logging extension to CoreWCF (`IDispatchMessageInspector`).

**What is not:**
- **No entry point.** No Program.cs, no `Main`, no `WebApplication.CreateBuilder` anywhere (0 grep hits) → CS5001; nothing can host anything.
- **All 102 root .asmx endpoints exist unchanged on both sides**; 100+ code-behinds still derive from `System.Web.Services.WebService` — an API that does not exist on .NET 8 and has no shim. No ServiceContract, no `AddServiceModelServices`, no SoapCore: "ASMX code parked under an unbuilt CoreWCF intention."
- `Web.config` is **byte-identical to master** (256 lines) including secrets; no appsettings.json; `Code/Global.asax.cs` is 98% commented out — startup wired to nothing. Pipeline (`azure-pipelines.yml`, `build.cake`) unchanged — still builds the .vbproj.
- All **7 ProjectReferences** (Amadeus, Galileo, Sabre, TravelPort, TripXMLMain, TripXMLTools, Worldspan) verified legacy net48 with no converted branch in any sibling — unbuildable by construction.
- Cruft: stale net48 `packages.config` still tracked (conflicting versions: TripXML.Core 1.0.16 vs 1.0.19, PaymentServices 1.0.30 vs 1.0.34), absolute HintPath to `C:\Program Files (x86)\...\v4.8\System.Data.dll`, OctoPack 3.6.5 (dead on .NET 8), VS-only TransformXml AfterBuild target, invalid converted VB in `My Project/`.
- Discrepancy vs the conversion claim "CompressionExtension replaced by NuGet packages": converted code-behinds still apply `[CompressionExtension()]` attributes with **zero** reference or package providing the type (verified).

**Master-commit sync status (24 master commits since merge-base):**

| Master commit | Change | Ported to converted? |
|---|---|---|
| `38a6b69` | LastTicketingDate | yes |
| `20885ba` | wsLowFarePlus_vJR + Newtonsoft 13 | yes |
| `706423d` | NDC property on OTA_TravelItineraryRS_v03 (2026-05-19) | **no** |
| `4931022` | ShowMileage AL decoding fix | **unverified — needs review** |
| `50ede6b`, `273797b` | Sabre NDC XSLT (lands via sibling-repo/binary updates) | n/a — requires sibling conversions |
| ~16 others | submodule/DLL/AssemblyFileVersion bumps | n/a — translate to "convert the siblings" |

**Verdict:** real translation work; cannot compile or serve a single endpoint. The host-side migration (entry point + SOAP stack for ~102 endpoints + 7 sibling conversions + the external packages) is the actual long pole.

---

## 4. Build verification (Linux, dotnet SDK 10.0.108 / 8.0.127)

| Target | Restore | Build |
|---|---|---|
| TripXMLLibrary | SUCCESS (~3s, nuget.org only) | **SUCCESS** — both net472 and net6.0, 0 errors / 1219 warnings; nupkg produced |
| wsTripXML-converted | SUCCESS (private feed reachable, HTTP 200; TripXML.Core 1.0.19 + PaymentServices 1.0.34 resolved, no credentials needed) | **FAILED** — NETSDK1022, then MSB3644 ×7 |

**TripXMLLibrary detail:** net472 reference assemblies auto-resolved via Microsoft.NETFramework.ReferenceAssemblies (no csproj changes needed). Warning profile: CS1591 (missing XML docs across public API), ~20× CA2200 (re-throw) in `cDA.cs`, and **NU1902/NU1903 — System.Data.SqlClient 4.8.3 has known moderate+high severity vulnerabilities**. All ~240 HintPath references to checked-in GDS XSL DLLs resolved (Windows-style backslash paths worked on Linux MSBuild). Compile success ≠ runtime success — the Assembly.Load path over those Framework DLLs is untested on net6.0 (§9).

**wsTripXML-converted detail:** Run 1 stops at NETSDK1022 — duplicate `Content` items (`nuget.config`, `Web.config` at `wsTripXML-converted/wsTripXML.csproj` lines 114/552 collide with Sdk.Web default globs; fix is deleting the explicit includes). Run 2 (diagnostic, default content disabled) reaches **MSB3644 ×7**: every sibling ProjectReference (Amadeus, Galileo, Sabre, TravelPort, TripXMLMain, TripXMLTools, Worldspan) is a legacy net48 project with no v4.8 targeting pack on Linux — and even if they built, net8.0 cannot consume net48 ProjectReferences. **The C# compiler never ran, so no code-level error inventory for the converted sources exists yet.** Residual landmines further down the same path: hardcoded Windows HintPath for System.Data (line 87), VS-only TransformXml task, OctoPack.

---

## 5. System topology & dependency hygiene

### Umbrella repo (`TripXML`)
- `TripXML.sln` (VS 18, Format 12.00): 13 buildable projects — 8 csproj + 1 vbproj inside gitlink dirs, 2 local test csproj (GalileoTests, TripXMLMainTests), and **2 outside the repo**: `..\..\..\technologytransferusa\TripXML.Core\TripXML.Core.csproj` and `...\PaymentServices\PaymentServices.csproj` (sln lines 471/473). The `technologytransferusa` path mirrors the author's machine layout — almost certainly a GitHub org folder; **the modern multi-targeted sources for TripXML.Core and PaymentServices are NOT in the TTC ADO org (verified org-wide)**.
- `TripXMLToolsTests/TripXMLToolsTests.csproj` is orphaned — in no solution at all (MSTest 2.1.1 vs 3.7.3 elsewhere).
- `TripXML.sln.bak` tracked: a VS-16-era pre-conversion snapshot (still lists AdminTools and TripXMLMain.vbproj).
- **15 gitlinks (mode 160000), no `.gitmodules` — ever** (verified in full history). Fresh clone = 15 empty dirs; `git submodule update --init` fails outright.

### Gitlink pin drift vs sibling master tips (verified via ADO)

| Submodule | Pinned @ | Behind master | Submodule | Pinned @ | Behind master |
|---|---|---|---|---|---|
| XSLs | 824247e4 | **3** | Portal | baa188ca | 0 |
| wsTripXML | 273797ba | **2** | SITA | 6f1722ea | 0 |
| AdminTools | 61efb838 | 1 | Sabre | 1fb9eb7a | 0 |
| Amadeus | dde793d2 | 1 | TravelPort | c7a55ac9 | 0 |
| TripXMLLibrary | 9da0c030 | 1 | TripXMLMain | ba984cb4 | 0 |
| CompressionExtension | 3ffd4517 | 0 | TripXMLTools | c47c0531 | 0 |
| Galileo | a1925a28 | 0 | Worldspan | e7e7e273 | 0 |
| **VirtualCreditCard/VirtualCreditCard** | c2b91ed9 | **DEAD** — repo deleted from the org; commit unfetchable anywhere (all 19 projects / 36 repos searched) | | | |

The drifted pins matter: the umbrella sln composes a wsTripXML 2 commits older and an XSLs 3 commits older than what the team is actually shipping — the umbrella's "Update submodule references to latest commits" workflow is manual and already stale.

### Inter-repo dependency edges (from per-repo analyses; build-time unless noted)

| From | To | Mechanism |
|---|---|---|
| wsTripXML (master) | Amadeus, CompressionExtension, Galileo, Sabre, TravelPort, TripXMLMain, TripXMLTools, Worldspan | ProjectReference `..\<repo>\...` |
| wsTripXML (master) | PaymentServices, TripXML.Core | ProjectReference `..\..\..\..\technologytransferusa\...` (4-up, exists on no audited machine) |
| wsTripXML@converted | same 7 siblings (CompressionExtension ref dropped, attributes kept) | ProjectReference; PaymentServices/TripXML.Core now NuGet |
| Amadeus, Galileo, Sabre, TravelPort, Worldspan, TripXMLTools | TripXMLMain | ProjectReference `..\TripXMLMain\TripXMLMain.csproj` |
| SITA, Portal | TripXMLMain (**broken**) | ProjectReference to deleted `TripXMLMain.vbproj` (lives only in TripXMLMain_VB, different GUID) |
| TripXMLMain | wsTripXML (9 DLLs), XSLs (2 DLLs) | 11 HintPaths escaping the repo; the 9 `..\wsTripXML\Xsl\Travelport\*.dll` are tracked nowhere — recoverable only from TripXMLMain's own history |
| TripXMLMain_VB | XSLs (369), wsTripXML (10) | 379 cross-repo binary HintPaths |
| PaymentServices | TripXMLMain | 22 HintPaths into `..\TripXMLMain\packages\` (sibling's NuGet restore folder) — unbuildable until a restore runs in the *sibling* repo |
| PaymentServices | TripXMLLibrary | `TripXML.Library 2.0.2` package ref (verified unused in code) |
| XSLs | wsTripXML, TripXMLMain | **push-style**: 24 .bat scripts xcopy xsltc-compiled DLLs into `..\..\wsTripXML\bin`, `..\..\TripXMLMain\Xsl\*`, plus absolute `C:\TripXML\Xsl\*` — checkout directory layout is load-bearing |
| AdminTools | wsTripXML | runtime SOAP client of `http://kiev/tripxml/*.asmx` (no build dependency) |
| GDS adapters (all) | XSLs content | runtime: load `<GDS>_*.xsl` stylesheets by path convention |
| TripXMLTools | wsTripXML | runtime: probes `AppDomain` loaded assemblies for the `wsTripXML` host |
| Umbrella tests | Galileo, TripXMLMain, TripXMLTools | ProjectReferences into gitlink dirs (empty on fresh clone) |

**wsTripXML is the hub consumer; TripXMLMain is the hub dependency.** Every migration path runs through TripXMLMain first and wsTripXML last.

### Binary/feed culture
- Checked-in compiled XSLT DLLs everywhere: TripXMLLibrary 372 (132 unreferenced by its csproj), umbrella 9, TripXMLMain 13 tracked (+372 in history; commit c43521d), TripXMLMain_VB ships `Xsl.zip` with 474 DLLs. All xsltc output = .NET Framework-only.
- Private feed `https://techtransnuget.azurewebsites.net` (anonymous read) hosts **37 packages**, incl. TripXML.Core 1.0.19 (20 versions), PaymentServices 1.0.34 (32 versions), TripXML.Library 2.0.2 (1), TripXML.GDSFunctions 1.0.3 (4). Cached TripXML.Core 1.0.19 / PaymentServices 1.0.34 multi-target net48+net6.0 — modern sources exist outside ADO.
- **CI**: exactly one registered pipeline in the ADO project — `wsTripXML` (YAML, id 22, 2023-08-18), which does an Azure-Repos-only `checkout: git://TripXML/<repo>` of 9 siblings and still builds the legacy vbproj. XSLs tracks an `azure-pipelines.yml` but no pipeline is registered for it or any other repo.

---

## 6. Security & GitHub-transfer red flags

**Plain statement: a history rewrite (filter-repo/BFG or fresh-history import) plus rotation of every credential below is required before any GitHub push.** Tip-deletion is insufficient — every item is in history; wsTripXML's credential store has 34 historical blob versions.

Worst items per repo (kind + path only, no values):

- **wsTripXML (master AND converted — Web.config byte-identical):**
  - `Web.config` — SQL connection string with user+password (host data.downtowntravel.com), `Password`, `VCCPassword` appSettings; converted analysis also found `HasuraKey` + raw-IP internal endpoints. SMTP credential pairs present in the mailSettings block (block is commented out per adversarial verification — still secret-bearing tracked text).
  - `Web.Release.config` — production SQL connection string + password replacement values (lines 10/14/18).
  - `build.cake` — hardcoded Octopus Deploy API key + internal server `http://georgia/octopus/`.
  - `Tables.zip` (1.5 MB, tracked) — `Tables/Users/DTT.xml`: ~208 username/password pairs incl. Amadeus **Production** PCC credentials; `tt_acl.xml`: 8 ACL service-account credential pairs. Uncompressed `DTT.xml` (~2 MB) in history across 35 commits back to the TFVC import.
  - History blobs >5 MB: `.vs/slnx.sqlite` 10.3 MB ×2, `bin/wsTripXML.pdb` 6.8 MB ×2.
- **TripXML (umbrella):** `XMLFile2.xml` (347 KB, repo root) — live **production** GDS Userid/Password + PCC + real PNR locator; `VirtualCreditCard/Postman/Virtual Creadit Cards (SOAP).postman_collection.json` — TripXML service credentials + Amadeus provider credentials, System=Production. The same password value is shared across both files and providers → **org-wide credential rotation, not just file scrubbing**.
- **AdminTools:** `AdminTools.cs` — 4 blocks of hardcoded **production** Amadeus credentials (lines 62-69, 1713-1720, 1746-1753, 1801-1808); commented `sa` SQL connection strings with a public IP (lines 40-43, 1474-1476); internal SMTP relay + admin emails; all present since the first import commit.
- **TripXMLLibrary:** `build.cake` line 52 — NuGet feed API key, **also packed inside the published TripXML.Library 2.0.2 nupkg** (contentFiles); `cDA.cs` line 1501 — commented SQL connection string with public IP + password.
- **PaymentServices:** `build.cake` line 52 — same feed API key (file byte-identical to TripXMLLibrary's), in all 4 commits touching it.
- **TripXMLMain:** `cDA.cs` line 1501 — same commented SQL credential; history-only `Xsl/Amadeus/Log/Log.txt` — cleartext Amadeus credentials (Training system) + a payment-card-shaped test number.
- **TripXMLMain_VB:** `cDA.vb` line 1688 — commented SQL connection string (public IP, user, password); `Xsl/Amadeus/Log/Log.txt` credentials duplicated inside tracked `Xsl.zip`; `WS_FTP.LOG` in zip leaks production/staging IPs; `obj/` NuGet artifacts leak developer paths (`C:\Users\akobelev\...`).
- **SITA:** `SITAAdapter.cs` line 116 — commented vendor-default certificate password; runtime design flaw: GDS passwords flow into trace logs on every request (`SITAAdapter.cs` lines 40/54/70/100).
- **Portal:** `PortalAdapter.vb` line 39 — hardcoded static API token (base64 GUID) in the CreateSession SOAP body.
- **TravelPort:** tracked `.vs/TravelPort/v16/.suo` (759 KB ×4 revisions) + `.vs/slnx.sqlite` — IDE state with developer-machine paths; historical committed build output.
- **XSLs:** no credentials; business-data exposure (`Amadeus/alliancetravel_BL.xml` client + production PCC + private fare code; `FailedBookingsList.xls` ×2).
- **Clean (full-history verified):** Sabre, Worldspan, TripXMLTools, Amadeus, CompressionExtension, Galileo — no secrets, but all carry TFS leftovers (`.vspscc` files, `Scc*=SAK` csproj properties) and `ttc.visualstudio.com` URLs in commit messages (these transfer verbatim to GitHub).
- Internal hostname/infra disclosure across repos: `kiev`, `ukraine`, `georgia` (Octopus), `data.downtowntravel.com`, `maillist.anaxanet.com`, `tripxml.downtowntravel.com` in WSDL/disco/config files; `ws://104.x` raw-IP endpoints.

### Pre-transfer action summary per repo

| Repo | History rewrite needed? | Why |
|---|---|---|
| wsTripXML (all branches) | **yes** | Tables/Users/DTT.xml ×34 blob versions, Web.config secrets, Octopus key, >5MB IDE/pdb blobs |
| TripXML (umbrella) | **yes** | XMLFile2.xml + Postman credentials in multiple historical blob versions |
| AdminTools | **yes** | Prod GDS creds + sa strings present since first import commit |
| TripXMLLibrary, PaymentServices | **yes** | Feed API key in every build.cake revision; cDA.cs credential |
| TripXMLMain, TripXMLMain_VB | **yes** | cDA credential; history-only Amadeus Log.txt creds (+ inside Xsl.zip) |
| SITA, Portal | cleanup only | Commented vendor password / static token at tip; single-commit history |
| Sabre, Worldspan, TripXMLTools, Amadeus, Galileo, CompressionExtension, XSLs, TravelPort | optional | No secrets (full-history verified where scanned); rewrite only to drop IDE/bin blobs + ADO URLs in commit messages |

---

## 7. Recommended migration order

Derived from the dependency graph (§5): leaf GDS libs depend only on TripXMLMain; wsTripXML consumes everything.

0. **Pre-work (gates everything):** rotate all §6 credentials; decide history strategy (rewrite vs fresh import); locate the `technologytransferusa` TripXML.Core/PaymentServices sources and bring them under org control — they are already net48+net6.0 and are the template the rest should follow. Remove the dead VirtualCreditCard gitlink; add `.gitmodules` or consolidate (§8).
1. **TripXMLMain** (hub): SDK-style multi-target (net48;net8.0). The xsltc XSL-assembly dependency (~285 refs) must be redesigned — runtime `XslCompiledTransform` over the XSL *sources* in XSLs (only 18 stylesheets in the live build path have VB msxsl:script blocks needing porting), since compiled XSLT assemblies cannot exist on modern .NET. This is the long pole; TripXML.Core on the feed may already be its successor — verify before porting (§9).
2. **Leaf GDS adapters** (each blocked only by TripXMLMain): order by effort — Worldspan, TravelPort, Sabre (no NuGet, no hard blockers beyond `HttpWebRequest` + the `_HttpResponse` private-reflection hack in Sabre/Worldspan, which silently breaks on modern .NET), then Galileo and Amadeus (ASMX client proxies must be regenerated with dotnet-svcutil/HttpClient; Amadeus additionally needs its `HttpContext.Current` host coupling removed). All need wildcard `AssemblyVersion("1.0.0.*")` fixes (CS8357) and Scc/ClickOnce cruft stripped.
3. **TripXMLTools** (depends on TripXMLMain, consumed by wsTripXML): move `WebConfigurationManager` to IConfiguration; fix the unauthenticated-JWT settings endpoint while there; beware SDK glob re-including the orphaned `TripXMLLog.cs`.
4. **wsTripXML host** (last): add Program.cs/host, choose the SOAP stack (CoreWCF or SoapCore) for the ~102 endpoints, migrate Web.config → appsettings + secrets store, replace CompressionExtension with response-compression middleware (and remove its attributes), port the missed master commits (`706423d`, verify `4931022`), rewrite the pipeline.
- **XSLs**: not a code migration but must convert from xsltc-compiled assemblies to runtime-loaded XSLT (with the 18 VB script blocks ported to extension objects) in step 1's redesign.
- **CompressionExtension**: retire rather than port (per-request gzip-in-SOAP predates HTTP compression; the type cannot exist on modern .NET) — but it cannot simply be deleted: wsTripXML references it by ProjectReference and applies its attribute per-method on both branches, so removal must be coordinated in step 4.
- **PaymentServices (ADO clone)**: do not migrate — reconcile with the external modern source that produces 1.0.34, then archive the clone.

### Archive candidates — justification from data

| Repo | Evidence for archiving |
|---|---|
| Portal | Zero consumers system-wide (grep across all repos); the only dispatch sites in wsTripXML are commented out and name a different class (`cServicePortal`); the Air XSLs its code requests never existed in any repo's history; references a deleted `TripXMLMain.vbproj`; untouched since the 2014 TFVC import; left out of the umbrella sln |
| SITA | Single-commit 2014 import; built on WSE 3.0 (`Microsoft.Web.Services3.SoapClient` + custom body-only X.509 signing filter that rules out default WCF/CoreWCF bindings); cannot build as cloned (WSE DLL untracked); only caveat: live `Case "SITA"` dispatch strings in wsTripXML (§9 #4) |
| TripXMLMain_VB | Superseded by the C# conversion in TripXMLMain; the full VB original is *also* preserved inside TripXMLMain's own history; repo is a credential/artifact tarpit (Xsl.zip, committed bin/obj/.vs, logged GDS creds) |
| AdminTools | Dormant since 2022-11; pure runtime *consumer* of wsTripXML's SOAP endpoints (zero build coupling); its entire 7-commit history is credential-poisoned — if the ticketing-automation function is still needed, regenerate the 4 WCF client proxies with dotnet-svcutil in a fresh repo |

---

## 8. GitHub transfer considerations

- **Submodules vs monorepo:** the umbrella analysis recommends **monorepo consolidation (Option B)** — subtree-merge the 14 live component repos into the umbrella at their existing paths, vendor TripXML.Core/PaymentServices in-tree, fix the 2 sln paths. Rationale: the components are not independently consumed (single sln, single deployable, cross-repo ProjectReferences and tests already), and the team has demonstrated the submodule failure mode for years (no `.gitmodules` ever; "Update submodules (dirty)" commits; pin drift in §5). If submodules are kept anyway (Option A), the non-negotiable minimum is: write `.gitmodules` with relative URLs, remove the dead VirtualCreditCard gitlink, and fix/remove the two external sln entries — otherwise the transferred umbrella is just as unclonable on GitHub.
- **Secret purge is a hard prerequisite** (§6): rewrite must cover **all branches**, not just master — wsTripXML alone has 5 unmerged branches (`converted`, `RepriceWithTravelport`, `feature/payment-services`, `feature/tripxml_object-caching`, `object_caching_platform`), of which only `converted` has been audited; the other 4 are unscanned (§9). TripXMLTools' two stale remote branches and Worldspan/TravelPort `feature/object-caching` are verified ancestors of master — safe to prune.
- **History size/quality:** no repo exceeds GitHub limits (largest history blobs: 10.3 MB slnx.sqlite, 6.8 MB pdb in wsTripXML); the same rewrite that purges secrets should drop committed `bin/`, `obj/`, `.vs/`, `.suo`, `Xsl.zip`, `Tables.zip` blobs and optionally move compiled XSL DLLs to LFS or delete them. `ttc.visualstudio.com` URLs in commit messages across most repos transfer verbatim — cosmetic, but a fresh-history import removes them too.
- **Destination org:** `wsTripXML.nuspec` already sets projectUrl to `https://github.com/Downtown-Travel-TT/wsTripXML`, and an Amadeus comment links a private `Downtown-Travel-TT` issue — a GitHub org already exists; `technologytransferusa` is likely a second org holding TripXML.Core/PaymentServices. Decide which org owns what before pushing.
- **Branch strategy:** before transfer, decide per branch — merge `converted` work forward or rebase it onto current master (it already misses `706423d`); scan then prune or transfer the 4 other wsTripXML branches; prune the verified-ancestor stale branches (TripXMLTools ×2, Worldspan, TravelPort). A blind `git push --mirror` transfers unscanned branches and their secrets.
- **Pipelines:** the `git://TripXML/<repo>` multi-checkout syntax in both tracked azure-pipelines.yml files is Azure-Repos-only and breaks on GitHub; the XSLs build additionally needs a self-hosted Windows runner with NETFX 4.8 Tools (xsltc) until step 1 of §7 eliminates it (its current ADO pool is already self-hosted: `pool: default`). The single registered pipeline (wsTripXML) still builds the legacy vbproj — there is no CI to preserve for the other 16 repos, so GitHub Actions can be designed fresh.
- **Feed/package strategy:** keep techtransnuget or move to GitHub Packages, but **rotate the feed API key regardless** (leaked in 2 repos' build.cake and inside the published TripXML.Library nupkg); re-publish TripXML.Library without the embedded key; note XSLs publishes prerelease-versioned (`1.0.0-CI-*`) packages via `byPrereleaseNumber` that consumers may pin. The feed is anonymous-read today — decide whether that survives the transfer.
- **Author identity mapping:** history contains personal-address commits (e.g. `antryas@gmail.com` in PaymentServices) and `TFVC Import <ImportService@microsoft.com>` roots — map or accept before rewrite, since the rewrite is the one chance to normalize.

---

## 9. Open questions

1. **Where do the `technologytransferusa` sources live, and who publishes the packages?** TripXML.Core 1.0.19 (20 versions) and PaymentServices 1.0.34 (32 versions) multi-target net48+net6.0 and are actively consumed, but their source repos are outside the TTC ADO org and no ADO pipeline publishes them. Until found, the system's two most-modern components are unauditable (secrets, license, bus factor).
2. **No compiler-level error inventory for `wsTripXML@converted`.** The build dies at NETSDK1022 → MSB3644 before csc runs; the "106 files on System.Web.Services / no entry point" findings are static inference. Next step: stub or convert the 7 sibling references and capture the real error list to size the remaining work.
3. **TripXML.Library runtime risk + missing consumer.** Its net6.0 build is green, but `CoreLib.TransformXML` does `Assembly.Load` over .NET-Framework xsltc-compiled DLLs — never executed on net6.0 (a 10-line smoke test would settle it). And no repo consumes the package: wsTripXML's packages.config references neither TripXML.Library nor XSLs; the only reference (PaymentServices) is unused. Is it the intended modern replacement for TripXMLMain's near-duplicate CoreLib/cDA, or dead weight?
4. **SITA: live or dead at runtime?** wsTripXML has uncommented `Case "SITA"`/`"sita"` dispatch (`Code/wsInventoryManagement.asmx.vb:120`, `Code/wsNative.asmx.vb:105`) yet no SITA reference at HEAD and the SITA repo cannot build. How the dispatch resolves (reflection load? manual DLL drop? dead path) decides between an expensive WSE3 rewrite and archiving.
5. **CompressionExtension on the converted branch is broken by design:** `[CompressionExtension()]` attributes are applied in converted code-behinds but no reference or package provides the type, and the "do not port" recommendation has no replacement plan on the net8 branch.
6. **VirtualCreditCard disposition:** gitlink pinned at c2b91ed9, repo deleted org-wide, commit unfetchable. Does PaymentServices' ConnexPay client fully supersede it, or is code permanently lost? The umbrella cannot be made clonable until this is answered or the gitlink removed.
7. **Unmerged-branch secret scans:** wsTripXML's 4 non-`converted` unmerged branches have never been scanned for unique code or secrets; a mirror push transfers them and the history rewrite must cover them. Amadeus's `origin/feature/object-caching` merge status is also unchecked.
8. **Canonical ASMX endpoint count:** sources disagree — 102 .asmx (conversion diff, root only) vs 104 tracked (102 root + 2 `App_Data/Admin`), and 100 vs 104 vs 106 code-behinds referencing System.Web.Services. The migration needs one authoritative endpoint inventory before the SOAP-stack rewrite is scoped.

---

*Sources: 18 adversarially verified per-repo analyses (corrections applied over original claims, verified additions merged), umbrella structural analysis, master-vs-converted conversion diff, 2 Linux build verifications, and independently verified ADO REST / NuGet feed facts (commit dates, pin drift, pipeline inventory, feed inventory, branch ancestry).*
