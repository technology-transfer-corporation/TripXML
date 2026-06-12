# Azure DevOps retirement checklist

The monorepo at this repository root replaces the 9 active Azure DevOps repos
(`dev.azure.com/TTC/TripXML/_git/*`). Their full history was imported on
2026-06-12 via `git-filter-repo --to-subdirectory-filter` (see
`monorepo-consolidation-design.md`), and committed secrets were scrubbed from
the imported history (`gitleaks` clean over all refs).

## Credential rotation (do this first)

The scrub removed secrets from **this** repo's history, but the originals still
exist in the ADO repos and were valid at scrub time. Rotate before or at
retirement:

- [ ] SQL login `ttlog` on `data.downtowntravel.com` (was committed in
      `Web.config` / `Web.Release.config` / `appsettings.json`)
- [ ] Hasura admin key for the REST endpoint (was committed in
      `appsettings.json`, ADO commit "Add Hasura Api Key")
- [ ] The ADO PAT in local `env.txt` (gitignored, never committed — rotate as
      hygiene when ADO access changes)

## ADO repos — archive, don't delete

For each of: TripXMLMain, Amadeus, Galileo, Sabre, TravelPort, Worldspan,
TripXMLTools, wsTripXML, XSLs:

- [ ] Set the repo to **Disabled** (Project Settings → Repositories) or at
      minimum add a branch policy locking `master`, after pointing the repo
      README at the monorepo.
- [ ] Leave the 8 already-archived repos as-is: AdminTools,
      CompressionExtension, PaymentServices, Portal, SITA, TripXML (umbrella),
      TripXMLLibrary, TripXMLMain_VB.

Note: the imported history excludes stale ADO feature branches (e.g.
`wsTripXML` `RepriceWithTravelport`, `object_caching_platform`) — they remain
retrievable from the archived ADO repos. The legacy VB tip of `wsTripXML
master` is preserved here as branch `wstripxml-vb-master`.

## Local workspace cleanup (after the GitHub push is verified)

- [ ] Delete `repos/` (the 18 local clones, including the
      `wsTripXML-converted` worktree) — everything active is in `src/` now.
- [ ] Remove the `repos/` entry from `.gitignore`.
- [ ] Delete the staging dir `D:\Work\TechTrans\tripxml-import\`.
- [ ] `baselines/` (legacy WSDLs, gitleaks reports) stays local and gitignored.

## Ongoing hygiene

- Run `gitleaks detect` in CI (config: `.gitleaks.toml` at root; the allowlist
  covers `SendTrace` banner false positives only).
- `appsettings.json` stays placeholders-only; real values via environment
  variables (`AppSettings__...`), user-secrets, or an uncommitted
  `appsettings.Production.json`.
