# ADR 0004: Static AppState facade over IMemoryCache replaces HttpApplicationState

Status: Accepted (2026-06-11)

## Context

The legacy ASMX application kept process-wide state in `HttpApplicationState` (`Application["key"]`): ACL tables, per-user XSD lookups, and the ttAirports/ttAirlines/ttEquipments decoding `DataView`s. `HttpApplicationState` does not exist on .NET 10. The consuming code is mechanically converted VB-origin C# that reaches for global state from anywhere: today there are 569 textual references to `AppState.Get/Set/Remove` across 101 `.cs` files under src/ (`grep -ro "AppState\.\(Get\|Set\|Remove\)" src --include=*.cs | wc -l`). Threading an `IMemoryCache` through constructors at that scale during the migration would have been high-risk mechanical churn that buries the real migration diffs.

## Decision

A static facade, `TripXMLMain.AppState` (src/TripXMLMain/AppState.cs), wrapping the host's `IMemoryCache` singleton:

- `AppState.Initialize(IMemoryCache)` is called once from `TripXMLRuntime.Initialize` (src/wsTripXML/Code/Classes/TripXMLRuntime.cs), which receives the same `IMemoryCache` registered by `builder.Services.AddMemoryCache()` in src/wsTripXML/Program.cs. `Get`/`Get<T>`/`Set`/`Remove` mirror the `Application` indexer.
- Entries are set with no expiration, matching `HttpApplicationState` semantics (state lives until overwritten or the process ends).
- The facade lives in the TripXMLMain hub project so GDS adapter assemblies (e.g. src/Amadeus/TravelServices.cs) read decoding tables without referencing the host.
- `TripXMLTools.TripXMLLoad.BuildDecodingDataViews` publishes the ttAirports/ttAirlines/ttEquipments `DataView`s into AppState at startup (wired in `TripXMLRuntime.Initialize`; commit 141e022), and `wsRefreshMem.wmRefreshMem` (src/wsTripXML/Code/Admin/wsRefreshMem.asmx.cs) reloads them at runtime.

## Alternatives considered

- **Constructor-injected `IMemoryCache`** — rejected for the migration pass. 101 files would change for zero behavioral gain, every change a chance to alter legacy semantics, and the diff noise would have made the migration unreviewable.
- **`HttpContext.Items` / scoped state** — wrong lifetime; this is process-global state shared across requests and background loads.
- **Distributed cache (Redis, etc.)** — changes semantics (serialization, expiry, network failure modes) and adds infrastructure for a single-process host. Nothing requires cross-process state today.

## Consequences

- Positive: the migration kept every legacy call site intact; `Application["x"]` → `AppState.Get("x")` was a mechanical, auditable rewrite. Startup table load and the wsRefreshMem reload path work as before.
- Negative: this is a deliberate, known DI antipattern — a static service locator over a singleton. Tests touching these paths must call `AppState.Initialize` with a real or fake `IMemoryCache` first; before initialization the accessors return `null`/`default` silently rather than throwing. Dependencies on shared state stay invisible in constructor signatures.
- **Do not undo casually.** Do not refactor AppState to constructor injection as a drive-by "cleanup" — 569 call sites in legacy code with implicit ordering assumptions is a dedicated, fully-regression-tested project, not a side quest. Do not add expiration or size limits to the entries; eviction of an ACL or decoding table mid-flight is a production incident, not a tuning win.
- Revisit when: the host becomes multi-process and genuinely needs shared state, or a funded refactor with endpoint-level regression coverage (beyond the 24 XSLT tests) takes on the VB-origin layer as a whole. The same criteria govern [ADR 0005](0005-modcore-config-bridge.md).

Related: [architecture](../architecture.md), [development guide](../development-guide.md), [ADR 0005 (modCore.config bridge)](0005-modcore-config-bridge.md).
