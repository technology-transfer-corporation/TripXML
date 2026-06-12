# ADR 0002: Runtime XslCompiledTransform with per-process cache replaces xsltc-precompiled assemblies

Status: Accepted (2026-06-11)

## Context

The legacy pipeline precompiled stylesheets with xsltc.exe into assemblies and relied on `msxsl:script` Visual Basic blocks embedded in stylesheets. Neither survives .NET 10: xsltc targets .NET Framework only, and `XslCompiledTransform` script support is a .NET Framework feature. The repo ships 553 `.xsl` files under src/XSLs, and transforms are on the hot path of every endpoint response.

## Decision

Compile stylesheets at runtime, once per process, and replace the script blocks with a CLR extension object:

- `CoreLib.TransformXML(string inputXml, string xslPath, string xslName, bool fromFile = false)` (src/TripXMLMain/CoreLib.cs) compiles via `XslCompiledTransform` and caches in `_xslCache`, a `ConcurrentDictionary<string, XslCompiledTransform>` keyed case-insensitively by stylesheet path. The in-code comment states the intent: "the cache replicates the performance profile of the retired xsltc-precompiled assemblies."
- Compilation uses `XsltSettings(enableDocumentFunction: true, enableScript: false)` — `document()` stays available, script stays off.
- `TtVbXsltFunctions` (src/TripXMLMain/TtVbXsltFunctions.cs) is registered on every transform via `XsltArgumentList.AddExtensionObject` under namespace `urn:ttVB` and replaces the VB script functions (`FctDateDuration`, `FctArrDate`, `ShortDateFormat`, `GetBirthDate`, `datenow`). XSLT binds extension functions by reflected method name, case-sensitively; the implementations call the same `Microsoft.VisualBasic` runtime overloads the scripts called.
- Commit 4cda4b7 removed the `msxsl:script` blocks from 27 stylesheets (28 files changed, +1/−351; the 28th change fixed an absolute dev-machine import path in src/XSLs/Galileo/Galileo_QueueRS.xsl).
- `CoreLib.ClearXslCache()` drops all compiled stylesheets; `wsRefreshMem.wmRefreshMem` (src/wsTripXML/Code/Admin/wsRefreshMem.asmx.cs) calls it, so edited `.xsl` files go live without a host restart — the precompiled-assembly approach required a rebuild and redeploy.
- TripXML.XsltTests (src/TripXMLMain/tests/TripXML.XsltTests, 24/24 passing) covers compile-all plus golden transforms.

## Alternatives considered

- **Keep xsltc-precompiled assemblies** — not possible; the toolchain is .NET Framework-only.
- **Build-time compilation via a custom MSBuild step** — rejected. Recreates a fleet of stylesheet DLLs, couples 553 stylesheets to the build, and kills the hot-reload property ops actually use via wsRefreshMem.
- **Rewrite transforms in C#** — rejected. 553 stylesheets of business logic; rewrite risk dwarfs the first-use compile cost.
- **Keep `msxsl:script` with `enableScript: true`** — not available on .NET 10, and script-in-stylesheet is a code-injection surface we are glad to drop.

## Consequences

- Positive: stylesheets are plain editable files; cache hits are effectively free. Measured in-process (JIT pre-warmed, trivial `<root/>` input, new stack only — no legacy baseline): first-call compile 78.28 ms for src/XSLs/Sabre/v03_Sabre_PNRReadRS.xsl (254 KB), 16.29 ms for src/XSLs/Aggregation/Markups_LowFareRS.xsl (31 KB), 2.28 ms for src/XSLs/Aggregation/Aggregation_AirAvailRS.xsl (2 KB); cached medians 0.016/0.006/0.003 ms. Caveat: the cached medians measure cache-hit overhead plus a transform over trivial input that matches no templates — they are not realistic payload transform costs. The valid headline: compile cost (2–78 ms, scaling with stylesheet size) is paid once per process per stylesheet, where xsltc paid it at build time.
- Negative: the first request that touches each stylesheet after process start or `ClearXslCache()` absorbs the compile; a malformed stylesheet now fails at first use, not at build (mitigated by the compile-all test).
- **Do not undo casually.** Do not set `enableScript: true`, do not rename or re-case `TtVbXsltFunctions` members (transforms break at runtime, not compile time), and do not construct `XslCompiledTransform` per call — the cache is the design.
- Revisit only if: transform startup cost becomes a measured production problem, at which point pre-warming the cache at startup is the first move, not precompilation.

Related: [performance](../performance.md), [migration report](../migration-report.md), [ADR 0004 (AppState, same hot-reload path via wsRefreshMem)](0004-appstate-facade.md).
