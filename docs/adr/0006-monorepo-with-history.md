# ADR 0006: Monorepo with imported history instead of 18 Azure DevOps clones

Status: Accepted (2026-06-12)

## Context

The .NET 10 migration was executed across 18 independent Azure DevOps clones; one logical change-set touched 9 of them. The full analysis, target layout, and phase-by-phase execution plan are documented in [docs/monorepo-consolidation-design.md](../monorepo-consolidation-design.md) — this ADR exists so the decision is discoverable from the ADR index and is not re-litigated.

## Decision

One repository (this one), assembled by importing the 9 active repos (TripXMLMain, Amadeus, Galileo, Sabre, TravelPort, Worldspan, TripXMLTools, wsTripXML, XSLs) with full history via `git filter-repo --to-subdirectory-filter src/<X>` plus unrelated-history merges — not submodules. Migration work landed as one commit per project (21a86e9, 487d7a1, 9e9c590, 85eafde, b569756, 141e022, 4cda4b7, bf710ba, 9b8f052). The imported history was scanned with gitleaks before publication (config: `.gitleaks.toml` at the root); the wsTripXML VB lineage is preserved as branch `wstripxml-vb-master`. ADO retirement steps — including the still-pending step of wiring gitleaks into CI as a push gate — are in [docs/ado-retirement.md](../ado-retirement.md).

## Consequences

- Positive: `git log -- src/<X>` reaches pre-import history; cross-cutting changes are single commits; the 8 archive repos stay on ADO read-only.
- Negative: pre-import commit hashes differ from the ADO originals (filter-repo rewrite).
- **Do not undo casually.** Do not split projects back into per-project repos or submodules, and do not re-point remotes at the retired ADO repos — they no longer receive changes.
- Revisit only if: a component needs an independent release/ownership boundary that path-scoped CI inside the monorepo cannot provide.
