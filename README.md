# TripXML

Travel-industry SOAP middleware. TripXML exposes 104 OTA-style SOAP endpoints (wire-compatible with the original ASMX services) and brokers requests to five GDS providers — Amadeus, Galileo, Sabre, TravelPort, Worldspan — through an XSLT transformation layer.

Migrated from .NET Framework 4.x (ASMX/IIS) to **.NET 10 (CoreWCF/Kestrel)** on 2026-06-11. See [docs/migration-report.md](docs/migration-report.md) for the full record.

## Layout

| Path | What it is |
|------|------------|
| `src/wsTripXML` | CoreWCF host — 104 SOAP endpoints on Kestrel |
| `src/TripXMLMain` | Core library — XSLT engine, AppState cache, config bridge |
| `src/Amadeus`, `src/Galileo`, `src/Sabre`, `src/TravelPort`, `src/Worldspan` | GDS adapter libraries |
| `src/TripXMLTools` | Settings service and startup data loading (Hasura) |
| `src/XSLs` | 553 XSLT stylesheets (request/response transforms, aggregation, business logic) |
| `tools/` | Contract generation pipeline (`Generate-Contracts.ps1`, `ContractScan`) |
| `docs/` | Documentation — start at [docs/README.md](docs/README.md) |

## Build, test, run

```powershell
dotnet build TripXML.slnx                                  # full solution, expect 0 errors
dotnet test src/TripXMLMain/tests/TripXML.XsltTests        # XSLT suite, expect 24/24
dotnet run --project src/wsTripXML                         # host on http://localhost:63072 / https://localhost:63071
```

The host boots with the placeholder `appsettings.json` (startup data loading is resilient to unreachable backends). Real configuration comes from `AppSettings__*` environment variables or user secrets — never commit live values. See [docs/development-guide.md](docs/development-guide.md).

## Documentation

- [docs/development-guide.md](docs/development-guide.md) — how to work in this repo (humans and AI agents): recipes, contract regeneration, testing, config.
- [docs/architecture.md](docs/architecture.md) — request lifecycle, XSLT engine, adapter pattern, and the wire-parity invariants that must not break.
- [docs/migration-report.md](docs/migration-report.md) — what the .NET 10 migration changed, per project, with verification evidence.
- [docs/performance.md](docs/performance.md) — performance analysis of the new stack (expected effects + measured numbers).
- [docs/adr/README.md](docs/adr/README.md) — architecture decision records.
- [MIGRATION_STATUS.md](MIGRATION_STATUS.md) — historical pre-migration audit (2026-06-10).
