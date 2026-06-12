# ADR 0005: modCore.config NameValueCollection bridge from IConfiguration

Status: Accepted (2026-06-11)

## Context

Legacy code read configuration through `ConfigurationManager.AppSettings` / `WebConfigurationManager` string keys from Web.config `<appSettings>`. The reads are scattered through VB-origin code — today 56 indexer reads via `modCore.config[...]` across 6 files (src/TripXMLMain/cDA.cs alone has 48), plus unqualified `config[...]` reads inside modCore itself (e.g. `config["TripXMLLogFolder"]` in `AddLog`). The new host configures itself with `IConfiguration` (appsettings.json + environment variables). The same constraint as [ADR 0004](0004-appstate-facade.md) applies: plumbing `IConfiguration` or `IOptions<T>` through every legacy call site during the migration was unacceptable mechanical risk.

## Decision

A settable static `NameValueCollection` bridge on the legacy module class:

- `modCore.config` (src/TripXMLMain/modCore.cs, line 25) carries the comment "Populated by the host at startup from IConfiguration (replaces ConfigurationManager.AppSettings)".
- `TripXMLRuntime.Initialize` (src/wsTripXML/Code/Classes/TripXMLRuntime.cs) copies the `AppSettings` section of `app.Configuration` into the collection key-by-key, then derives `modCore.XslPath`, `modCore.LogPath`, and `modCore.SchemaPath` from it. Web.config `<appSettings>` keys map 1:1 to the `"AppSettings"` section of src/wsTripXML/appsettings.json (placeholders only — real values are deployment-side).
- Legacy adapters keep reading string keys unchanged. Example: src/Amadeus/ttHttpWebClient.cs reads `modCore.config["AmadeusSkipCertValidation"]` to gate per-handler TLS certificate validation bypass (the legacy code disabled validation process-wide via `ServicePointManager`; the bridge key lets ops re-enable it without a code change).
- TripXMLTools dropped `WebConfigurationManager` for the same bridge in commit 141e022 (6 files, +66/−352).

## Alternatives considered

- **`IOptions<T>` typed configuration** — rejected for the migration pass. It requires both DI plumb-through (the ADR 0004 problem) and inventing typed models for dozens of loosely-typed string keys whose semantics live in legacy call sites.
- **Inject `IConfiguration` directly** — same plumb-through problem; also couples class libraries (TripXMLMain, Amadeus, ...) to Microsoft.Extensions.Configuration when all they need is string lookup.
- **`System.Configuration.ConfigurationManager` compat package** — works on .NET 10 but reads `app.config` XML, creating a second config system next to appsettings.json with no environment-variable layering. One source of truth wins.

## Consequences

- Positive: zero changes at legacy read sites; configuration has a single source (`IConfiguration`) with JSON + environment-variable layering; tests and tools can set `modCore.config` directly because the property is settable.
- Negative: deliberately the same static-global antipattern as ADR 0004. Keys are stringly typed — a typo returns `null`, not an error. Reads before `TripXMLRuntime.Initialize` runs see an empty collection. Key names (`AmadeusSkipCertValidation`, `TripXMLFolder`, `XslPath`, `TripXMLLogFolder`, `SchemaPath`, ...) are an operational contract with deployment config.
- **Do not undo casually.** Do not convert call sites to `IOptions<T>` piecemeal — a half-migrated config system is worse than either endpoint. Do not rename keys in code without updating the `AppSettings` section contract; deployments break silently (`null` fallback paths). Keep src/wsTripXML/appsettings.json placeholders-only; real credentials never enter the repo.
- Revisit when: the criteria of ADR 0004 are met — a funded, regression-covered refactor of the VB-origin layer — and do both bridges in the same effort.

Related: [development guide](../development-guide.md), [ADR 0004 (AppState facade)](0004-appstate-facade.md), [ADR 0003 (AmadeusSkipCertValidation consumer)](0003-handrolled-soap-clients.md).
