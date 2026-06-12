# TripXML Monorepo Consolidation Plan

## Context

The .NET 10 migration is complete (slnx green, 104/104 endpoints, 92/92 WSDL parity) but the workspace is 18 independent Azure DevOps clones under gitignored `repos/`, with ~260 uncommitted migration files spread across 9 of them, and a meta-repo with **zero commits**. The user wants one unified repository that preserves the old git history. Brainstormed and locked with the user:

- **Scope:** the 9 active repos only — TripXMLMain, Amadeus, Galileo, Sabre, TravelPort, Worldspan, TripXMLTools, wsTripXML, XSLs. The 8 archive repos (AdminTools, CompressionExtension, PaymentServices, Portal, SITA, TripXML umbrella, TripXMLLibrary, TripXMLMain_VB) stay on ADO as read-only reference.
- **Structure:** true **monorepo with imported history** (git-filter-repo path-rewrite + merge), NOT submodules. Chosen over submodules because the migration itself proved the pain: one logical change-set touched 9 repos.
- **Hosting:** **GitHub (private), after a secrets scan** of the assembled history. No push happens before the scan gate passes.
- **Migration commits:** land **in the monorepo** — old repos imported at their last-committed tips, migration work becomes the monorepo's first content commits. ADO repos stay untouched.

Verified facts: total history ≈ 1,600 commits / ~37 MB across the 9 repos — trivially importable. Meta-repo branch `main`, no remote, gitignore = env.txt, repos/, baselines/, build-output.log. wsTripXML has branches `master` (315 c, VB legacy) and `converted` (299 c, the conversion lineage; worktree `repos/wsTripXML-converted` has the 179 dirty migration files) + 1 tag `1.1.234.251` + stale feature branches (not imported). `git-filter-repo` is NOT installed. `XslTestPaths.cs` walks up for a sibling `XSLs` folder — works under `src/` unchanged.

## Target layout

```
TripXML/                      (this meta-repo becomes the monorepo)
├── TripXML.slnx              # paths repos/ → src/
├── MIGRATION_STATUS.md
├── .gitignore                # keep env.txt, baselines/, build-output.log, .vs/; + bin/obj; repos/ stays until clones deleted
├── tools/                    # Generate-Contracts.ps1, Convert-Services.ps1, ContractScan
├── docs/                     # this design doc (copied from plan)
└── src/
    ├── TripXMLMain/          # full 91-commit history (tests/TripXML.XsltTests stays nested)
    ├── Amadeus/ Galileo/ Sabre/ TravelPort/ Worldspan/
    ├── TripXMLTools/
    ├── wsTripXML/            # 'converted' lineage = mainline; VB master tip kept as branch ref
    └── XSLs/                 # 368-commit history
```

Siblings stay siblings → all `..\TripXMLMain\`, `..\XSLs\` ProjectReference/Content relative paths keep working with zero edits.

## Phase 1 — Tooling + staged history rewrite

1. Install git-filter-repo: `pip install git-filter-repo` (or `scoop install git-filter-repo`); verify `git filter-repo --version`.
2. Staging dir **outside** the workspace: `D:\Work\TechTrans\tripxml-import\`.
3. For each of the 9 repos: `git clone <workspace>\repos\<X> staging\<X>` (fresh local clone — filter-repo requires fresh), then `git -C staging\<X> filter-repo --to-subdirectory-filter src/<X>`.
   - **wsTripXML special case:** after cloning, `git branch converted origin/converted` so filter-repo rewrites both `master` and `converted` heads. Tag `1.1.234.251` rides along (no collision risk — only tag in the set). Stale feature branches are not materialized.
   - Folder name for the host is `src/wsTripXML` (not `-converted`).

## Phase 2 — Assemble the monorepo

1. In the meta-repo (branch `main`, zero commits): commit 1 = updated `.gitignore` (add `bin/`, `obj/`, keep existing entries incl. `repos/` for now).
2. Per project (9×):
   `git remote add imp staging\<X>` → `git fetch imp` → `git merge --allow-unrelated-histories imp/master -m "Import <X> history into src/<X>"` → `git remote remove imp`.
   - wsTripXML: merge `imp/converted` instead; additionally `git branch wstripxml-vb-master imp/master` to preserve the VB master tip (contains the hand-ported 706423d NDC + 4931022 ShowMileage commits).
3. Gate: `git log --oneline -- src/Sabre` shows pre-import commits; total commit count ≈ 1,600 + 10.

## Phase 3 — Land the migration work

1. Per project: `robocopy repos\<X> src\<X> /MIR /XD .git bin obj .vs` (source for the host is `repos\wsTripXML-converted`). `/MIR` propagates deletions (e.g. removed AssemblyInfo.cs).
2. `git add -A src/<X>` + one descriptive commit per project ("TripXMLMain: migrate to .NET 10 — SDK csproj, runtime XSLT engine, AppState, Microsoft.Data.SqlClient", etc.). 9 commits.
   - **Verification per commit:** `git diff --name-only HEAD~1 -- src/<X>` must match the known dirty list from `git -C repos\<X> status --porcelain` (20/7/11/3/3/3/6/179/28 files). Investigate any extra/missing file before committing (likely line-ending noise — do NOT renormalize in this pass).
3. Root commit: `TripXML.slnx` (10 path edits `repos/...` → `src/...`, host entry → `src/wsTripXML/wsTripXML.csproj`), `tools/` (fix `$HostRoot` defaults in Generate-Contracts.ps1:15 + Convert-Services.ps1:18 and the ProjectReference in tools/ContractScan/ContractScan.csproj:12 → `..\..\src\wsTripXML\...`; delete stale `tools/ContractScan/obj+bin` artifacts), `MIGRATION_STATUS.md`, `docs/` with the design doc.
4. Gate: `dotnet build TripXML.slnx` → 0 errors; `dotnet test src/TripXMLMain/tests/TripXML.XsltTests` → 24/24 (XslTestPaths finds src/XSLs by walk-up; baselines/ still at root).

## Phase 4 — Secrets scan + history rewrite (hard gate before any push)

1. `gitleaks detect --log-opts="--all"` (install via scoop/winget) over the assembled monorepo. Legacy web.config-era connection strings/credentials are *expected* in old history.
2. Triage findings → build `replace-text` map (`literal-secret==>***REMOVED***`) → `git filter-repo --replace-text` on the monorepo. Free at this point: no remote, no downstream clones. Verify `wstripxml-vb-master` and the tag survive the rewrite (filter-repo rewrites all refs).
3. Re-run gate: gitleaks clean (or every remaining finding explicitly accepted as non-secret), `dotnet build` still green (working tree only changes if a *current* file contained a secret — appsettings.json has placeholders only, so expected unchanged).

## Phase 5 — Publish + retire

1. Create private GitHub repo (`gh repo create` — ask user for org/name at execution time, or default `renatbabin/TripXML` private).
2. Push `main`, `wstripxml-vb-master`, tags.
3. Fresh-clone verification: clone to a temp dir, `dotnet build` green, `git log --oneline | Measure-Object -Line` matches, `git log src/Sabre/ttHttpWebClient.cs` shows old history.
4. Document (not execute) the retirement steps in docs/: lock/archive the 9 ADO repos, delete local `repos/` clones + the wsTripXML-converted worktree when confident, then drop `repos/` from .gitignore.

## Decisions taken (no further input needed)

- Only mainline refs imported (master / converted); stale ADO feature branches remain in the archived ADO repos.
- No line-ending renormalization in this pass (avoid polluting the migration diffs); `.gitattributes` hygiene is a separate future task.
- Test project stays nested under `src/TripXMLMain/tests/` (its history lives there); relocation is a trivial later refactor if wanted.
- `baselines/` and `env.txt` remain gitignored and never enter history.

## Risks

| Risk | Mitigation |
|---|---|
| filter-repo install friction on Windows (needs Python) | scoop fallback; worst case `git subtree add` (works but old commits keep original paths → needs `--follow`) |
| robocopy /MIR drops or adds unexpected files | per-project diff-vs-dirty-list check before each commit |
| secrets in 1,600 imported commits | Phase 4 is a hard gate before any remote exists |
| hidden repos/ path assumptions | grep-verified: only slnx + 2 scripts + ContractScan.csproj; build+test gate catches stragglers |

## Verification summary

Phase 2: history reachable per subdirectory. Phase 3: build 0 errors + 24/24 tests + per-project diff parity. Phase 4: gitleaks clean. Phase 5: fresh clone from GitHub builds green.
