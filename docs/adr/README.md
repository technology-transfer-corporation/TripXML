# Architecture Decision Records

Deliberate decisions from the .NET 10 migration (executed 2026-06-11) and the monorepo consolidation. Each ADR names the code it governs and states explicitly what must not be undone casually — read the relevant ADR **before** refactoring AppState, the SOAP clients, the XSLT engine, or the CoreWCF contracts.

| ADR | Title | Status |
|-----|-------|--------|
| [0001](0001-corewcf-over-rest-rewrite.md) | Adopt CoreWCF to preserve ASMX SOAP wire contracts | Accepted (2026-06-11) |
| [0002](0002-runtime-xslt-compilation.md) | Runtime XslCompiledTransform with per-process cache replaces xsltc-precompiled assemblies | Accepted (2026-06-11) |
| [0003](0003-handrolled-soap-clients.md) | Hand-rolled SOAP/HTTP clients on shared HttpClient instead of generated proxies | Accepted (2026-06-11) |
| [0004](0004-appstate-facade.md) | Static AppState facade over IMemoryCache replaces HttpApplicationState | Accepted (2026-06-11) |
| [0005](0005-modcore-config-bridge.md) | modCore.config NameValueCollection bridge from IConfiguration | Accepted (2026-06-11) |
| [0006](0006-monorepo-with-history.md) | Monorepo with imported history instead of 18 Azure DevOps clones | Accepted (2026-06-12) |

## Adding an ADR

1. Take the next sequential number: `NNNN-short-kebab-title.md` in this directory.
2. Use the MADR-lite skeleton — `# ADR NNNN: title`, `Status: Proposed | Accepted (date) | Superseded by NNNN`, then `## Context`, `## Decision`, `## Alternatives considered`, `## Consequences` (positive, negative, the explicit "do not undo casually" consequence, and what would have to be true to revisit).
3. Verify every file path, symbol name, and number against the working tree before it goes in; cite commits by short hash.
4. Add the row to the table above and, if the decision affects day-to-day work, link it from [docs/README.md](../README.md) and the relevant guide ([architecture](../architecture.md), [development guide](../development-guide.md)).

Superseding: never rewrite an Accepted ADR's Decision — write a new ADR and mark the old one `Superseded by NNNN`.
