# Lopah Library

Working map for agents and contributors maintaining these shared .NET libraries.

## Skills and tracker

- **Forgejo:** before reading issue requirements, reviewing a pull request (PR),
  or updating tracker records, load the repo-local `forgejo` skill or follow
  [its workflow](./.agents/skills/forgejo/SKILL.md). The repository is
  `lopah/Library` at <https://git.lopah.eu/lopah/Library>, not GitHub.
- **Documentation:** before writing comments, XML documentation, Markdown,
  commit messages, or PR descriptions, load the repo-local `documentation`
  skill or follow [its standard](./.agents/skills/documentation/SKILL.md).
- **C# 15:** when planning the .NET 11 upgrade, working in a `net11.0` project,
  or using C# 15 features, load the repo-local `csharp-15` skill or read
  [its reference](./.agents/skills/csharp-15/SKILL.md). Check the project's
  current framework and language version before applying its examples.

For issue-driven changes, retrieve the description and discussion before
implementing. Clarify decisions that affect public contracts or consuming
applications before choosing a breaking approach.

## Boundaries

- `src/Api/` contains ASP.NET Core middleware and registration extensions.
- `src/OpenApi/` contains Swagger configuration, attributes, filters, and
  options. Its integration with the application pipeline lives in `src/Api/`.
- `src/Validation/` contains reusable validation attributes.
- `src/Utilities/` contains extensions, JSON converters, and naming policies.
- `tests/` has API, Utilities, and Validation test projects. OpenAPI integration
  coverage belongs to `tests/Lopah.Library.Api.Tests/`; there is no separate
  OpenAPI test project.

These are NuGet libraries, not an executable API or frontend. Preserve consumer
compatibility when changing public members, option defaults, serialization,
validation, or generated OpenAPI contracts.

Read [src/AGENTS.md](./src/AGENTS.md) for library changes and
[tests/AGENTS.md](./tests/AGENTS.md) for test changes.

## Development and verification

Run .NET commands from the repository root. Use the SDK selected by
`global.json` and tools from `.config/dotnet-tools.json` when those files are
present; otherwise inspect the project and build configuration for requirements.
Follow `.editorconfig`; keep formatting changes limited to the files and lines
needed for the task.

Run the relevant tests after changing behavior, then verify the full solution:

```bash
dotnet restore Lopah.Library.sln
dotnet build Lopah.Library.sln --configuration Release --no-restore
dotnet test Lopah.Library.sln --configuration Release --no-build
```

Restore local tools first when the task needs them. Consult the README's
development guide when present for coverage and packing instructions. Report
commands run, failures, and anything not verified.

## CI and releases

The repository is transitioning from GitHub Actions/Nuke to Forgejo Actions
and plain .NET CLI commands. Inspect the current checkout: use
`.forgejo/workflows/` when present; older revisions have `.github/workflows/`
and `build/`. Follow the active workflow for CI checks and publishing conditions
rather than assuming the migration has already landed.

Read `GitVersion.yml` before choosing version-bump commit wording; its custom
patterns differ from the portal's Conventional Commits standard. Explain
consumer migration steps in the README and release-facing changes in
`CHANGELOG.md` when applicable.

Packing locally verifies package creation. Publishing packages, creating
releases, pushing release-triggering branches, or merging requires the user's
request; a local verification task does not authorize a release.
