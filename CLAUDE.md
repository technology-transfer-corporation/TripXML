# TripXML

Travel-industry SOAP middleware on **.NET 10**: `src/wsTripXML` is a CoreWCF/Kestrel host exposing 104 SOAP endpoints wire-compatible with the original ASMX services; it brokers requests to five GDS providers (Amadeus, Galileo, Sabre, TravelPort, Worldspan) through their adapter libraries and the XSLT engine in `src/TripXMLMain`. Migrated from .NET Framework 4.x on 2026-06-11.

## Commands

```powershell
dotnet build TripXML.slnx                                  # expect 0 errors
dotnet test src/TripXMLMain/tests/TripXML.XsltTests        # expect 24/24
dotnet run --project src/wsTripXML                         # http://localhost:63072
```

## Required reading before changing code

- `docs/development-guide.md` — recipes (endpoints, adapters, XSLTs), contract regeneration, config/secrets rules.
- `docs/architecture.md` — request lifecycle and the **wire-parity invariants** section: the list of things that MUST NOT change (contract serialization attributes, `TripXML` header, `{m}Result` naming, RequestElement routing, Galileo envelope byte-rules, en-US culture pinning).
- `docs/adr/README.md` — decision records. Do not undo an ADR-documented decision (AppState facade, modCore.config bridge, hand-rolled SOAP clients, runtime XSLT compilation) without revisiting the ADR.

## Hard rules

- `src/wsTripXML/Code/Generated/*.g.cs` is generated — never hand-edit; regenerate with `tools/Generate-Contracts.ps1` and pre-flight with `tools/ContractScan`.
- `appsettings.json` is placeholders-only. Never commit real connection strings, keys, or endpoints. Real values come from `AppSettings__*` environment variables or user-secrets.
- Keep this file and `AGENTS.md` byte-identical (the GitNexus block below is auto-managed in both).
- For C# symbol-level impact analysis, the cwm-roslyn-navigator MCP tools (`find_references`, `find_callers`, `get_dependency_graph`) complement GitNexus with Roslyn-level accuracy.

<!-- gitnexus:start -->
# GitNexus — Code Intelligence

This project is indexed by GitNexus as **TripXML** (17318 symbols, 25975 relationships, 281 execution flows). Use the GitNexus MCP tools to understand code, assess impact, and navigate safely.

> Index stale? Run `node .gitnexus/run.cjs analyze` from the project root — it auto-selects an available runner. No `.gitnexus/run.cjs` yet? `npx gitnexus analyze` (npm 11 crash → `npm i -g gitnexus`; #1939).

## Always Do

- **MUST run impact analysis before editing any symbol.** Before modifying a function, class, or method, run `impact({target: "symbolName", direction: "upstream"})` and report the blast radius (direct callers, affected processes, risk level) to the user.
- **MUST run `detect_changes()` before committing** to verify your changes only affect expected symbols and execution flows. For regression review, compare against the default branch: `detect_changes({scope: "compare", base_ref: "main"})`.
- **MUST warn the user** if impact analysis returns HIGH or CRITICAL risk before proceeding with edits.
- When exploring unfamiliar code, use `query({query: "concept"})` to find execution flows instead of grepping. It returns process-grouped results ranked by relevance.
- When you need full context on a specific symbol — callers, callees, which execution flows it participates in — use `context({name: "symbolName"})`.

## Never Do

- NEVER edit a function, class, or method without first running `impact` on it.
- NEVER ignore HIGH or CRITICAL risk warnings from impact analysis.
- NEVER rename symbols with find-and-replace — use `rename` which understands the call graph.
- NEVER commit changes without running `detect_changes()` to check affected scope.

## Resources

| Resource | Use for |
|----------|---------|
| `gitnexus://repo/TripXML/context` | Codebase overview, check index freshness |
| `gitnexus://repo/TripXML/clusters` | All functional areas |
| `gitnexus://repo/TripXML/processes` | All execution flows |
| `gitnexus://repo/TripXML/process/{name}` | Step-by-step execution trace |

## CLI

| Task | Read this skill file |
|------|---------------------|
| Understand architecture / "How does X work?" | `.claude/skills/gitnexus/gitnexus-exploring/SKILL.md` |
| Blast radius / "What breaks if I change X?" | `.claude/skills/gitnexus/gitnexus-impact-analysis/SKILL.md` |
| Trace bugs / "Why is X failing?" | `.claude/skills/gitnexus/gitnexus-debugging/SKILL.md` |
| Rename / extract / split / refactor | `.claude/skills/gitnexus/gitnexus-refactoring/SKILL.md` |
| Tools, resources, schema reference | `.claude/skills/gitnexus/gitnexus-guide/SKILL.md` |
| Index, status, clean, wiki CLI commands | `.claude/skills/gitnexus/gitnexus-cli/SKILL.md` |

<!-- gitnexus:end -->
