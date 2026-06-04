# CI/CD Pipeline — HardToModifyRuntimeConstants

Event-driven pipeline (no cron). Workflows live here; their helper scripts live
in `scripts/`.

| File | Trigger | Purpose |
|------|---------|---------|
| `ci.yml` | push + PR on `main` + `workflow_call` | Build the solution and run both NUnit test projects on ubuntu + windows |
| `release.yml` | **manual dispatch** | Build the app, then cut the dated `vyyyyMMdd` Release |
| `nightly.yml` | successful CI on `main` + manual | Publish `nightly-yyyyMMdd` prerelease and prune old ones |
| `_build.yml` | `workflow_call` (internal) | Publish the primary app tarball |
| `scripts/version.pl` | invoked by workflows | Stamp each project's own `<Version>` + its folder's commit count (`--stamp`) |
| `scripts/update-changelog.mjs` | invoked by workflows | Bucketise commits into release notes by `+ - * # !` prefix |
| `scripts/prune-nightlies.mjs` | invoked by workflows | GFS retention: 7 daily + 4 weekly + 3 monthly |

## Notes

- **Tests are required.** CI runs both NUnit projects —
  `HardToModifyRuntimeConstants.Tests` and `Obfuscator.Tests` — on ubuntu and
  windows; either failing fails the build.
- **`PerformanceBenchmarks` is a BenchmarkDotNet app, not a test project**, so
  it is never run in CI.
- **No NuGet.** This repo ships no package; there is no pack or push step.
- **Versioning — files drive, never tags.** `version.pl --stamp` appends each
  project's folder commit count to its own `<Version>`. There is no single repo
  version, so the repo-level Release/tag is the date marker `vyyyyMMdd`.
