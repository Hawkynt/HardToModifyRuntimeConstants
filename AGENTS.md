# Agent guide — HardToModifyRuntimeConstants

Working agreement for **all** coding agents and human contributors working in
this repository. These rules are not optional. The full house spec lives in
the `Hawkynt/project-template` repo (`STANDARD.md`); this file is the
per-repo distillation.

## What this is

A C# **proof-of-concept for tamper-resistant runtime constants**: the
`ConstantObfuscator` build-time codegen tool, the runtime library, tests, and
`PerformanceBenchmarks`. The build-time/runtime separation is load-bearing —
the obfuscator is invoked via a codegen target, never referenced at runtime.

## Commits

- **Group changes semantically/logically** — one concern per commit. Keep
  the diagnostic commit-body style (symptom → root cause → fix).
- **Every subject line starts with a prefix**: `+` added · `-` removed ·
  `*` changed · `#` bug fixed · `!` critical todo.
- Never start a subject with "fix"/"bugfix"/"changed"/"modified".
- **No AI traces anywhere**: no `Co-Authored-By` AI lines, no "Generated
  with" footers, no agent mentions in messages, comments, or authorship.

## The loop (always, in this order)

1. **Before committing**: `dotnet build HardToModifyRuntimeConstants.sln -c
   Release` and `dotnet test HardToModifyRuntimeConstants.Tests` +
   `Obfuscator.Tests` until green. MSBuild-target changes must be verified
   on a CLEAN checkout (`git clean -xdf` first) — restore-ordering bugs only
   show there. Benchmark-affecting changes get a PerformanceBenchmarks run.
2. **Commit** (rules above) and **push**.
3. **Wait for CI**; on `main` a green CI triggers the nightly (prerelease +
   GFS prune, same-day replace). Fix and loop until everything is green.

Stable releases are **manual** (`gh workflow run release.yml`) — never cut
one unless explicitly asked.

## Code conventions

- Latest C# features; AOT-publish compatibility is part of the contract —
  no runtime reflection over the obfuscated constants.
- The security analysis in the README is the spec: every weakening or
  strengthening of the tamper-resistance gets that section updated in the
  same commit.

## README & repo conventions

- Standard frame: title → badges → one-line `>` blockquote (no Overview
  header); fixed emoji mapping for the standard sections
  (`## 🚀 Sample Usage`, `## 🛠️ Build and Test`, `## ❤️ Support`,
  `## 📜 License`).
- License is LGPL-3.0-or-later; the `## ❤️ Support` section and
  `.github/FUNDING.yml` stay intact.
