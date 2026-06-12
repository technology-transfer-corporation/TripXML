# TripXML documentation index

Documentation for the TripXML monorepo: a .NET 10 / CoreWCF port of the legacy ASMX SOAP host
(104 endpoints) plus its five GDS adapters, hub libraries, and 553 XSLT stylesheets. Start with
the development guide; everything else is reference.

## Start here

| Document | Purpose | Read this when... |
|---|---|---|
| [development-guide.md](development-guide.md) | Build/run/test quickstart, repo map, contract regeneration pipeline, step-by-step recipes, configuration and secrets policy, the canonical known-gaps list. | You are about to change anything in this repo — human or agent, this is the entry point. |

## Reference

| Document | Purpose | Read this when... |
|---|---|---|
| [architecture.md](architecture.md) | How the system works now: host composition, generated contracts, wire-parity invariants, routing behaviors, startup sequence, GDS adapter shape. | You need to understand a component before changing it, or you are walking the wire-parity checklist for an endpoint change. |
| [migration-report.md](migration-report.md) | What the 2026-06 .NET Framework to .NET 10 migration changed, per project, with commit evidence and the regressions/fixes called out plainly. | You hit behavior that differs from legacy and need to know whether it was deliberate, or you are reviewing the migration itself. |
| [performance.md](performance.md) | Measured numbers on the new stack (cold start, wsPing round-trip, XSLT compile-vs-cached) with conditions and caveats. No legacy-side baseline exists. | You are about to claim, optimize, or regress performance — read the caveats before quoting any number. |

## Architecture Decision Records

| Document | Decision |
|---|---|
| [adr/README.md](adr/README.md) | ADR index and process. |
| [adr/0001-corewcf-over-rest-rewrite.md](adr/0001-corewcf-over-rest-rewrite.md) | Keep the SOAP wire format on CoreWCF instead of rewriting to REST. |
| [adr/0002-runtime-xslt-compilation.md](adr/0002-runtime-xslt-compilation.md) | Compile XSLT at runtime (cached per process) instead of the retired build-time xsltc DLLs. |
| [adr/0003-handrolled-soap-clients.md](adr/0003-handrolled-soap-clients.md) | Hand-rolled SOAP clients over static pooled HttpClient in the GDS adapters. |
| [adr/0004-appstate-facade.md](adr/0004-appstate-facade.md) | `TripXMLMain.AppState` (IMemoryCache facade) replaces HttpApplicationState. |
| [adr/0005-modcore-config-bridge.md](adr/0005-modcore-config-bridge.md) | `modCore.config` NameValueCollection bridge from IConfiguration's AppSettings section. |
| [adr/0006-monorepo-with-history.md](adr/0006-monorepo-with-history.md) | One monorepo with imported per-repo git history, not submodules. |

## Historical / operational

| Document | Purpose | Read this when... |
|---|---|---|
| [monorepo-consolidation-design.md](monorepo-consolidation-design.md) | The executed plan that assembled this monorepo from 9 ADO repos with history, including the secrets-scan gate. | You need to know how history got here, why paths look the way they do, or how the import was verified. |
| [ado-retirement.md](ado-retirement.md) | Checklist for retiring Azure DevOps: credential rotation, repo archival, local `repos/` cleanup. | You are doing the ADO decommission work or rotating the exposed legacy credentials. |
| [../MIGRATION_STATUS.md](../MIGRATION_STATUS.md) | **Historical.** The pre-migration audit of 2026-06-10, preserved unchanged as the "before" record. | You want the state of all 17 legacy repos before the migration — not the current state. |
