# Documentation profile

For agents applying the shared
[`documentation` skill](https://git.lopah.eu/lopah/agent-skills/src/branch/main/plugins/core/skills/documentation/SKILL.md)
in Lopah Library. Where this page and the shared skill disagree, follow this
page.

## Where knowledge lives

| Knowledge | Location |
| --- | --- |
| Public type or member contract | C# XML documentation, written for consuming applications |
| Consumer setup, configuration, or migration | `README.md`, or a focused `docs/<topic>.md` when a topic outgrows it |
| Release-facing behavior change | `CHANGELOG.md`, following its existing format |
| A cross-library decision | ADR in `docs/adr/`, when needed |
| JSON options | The owning Markdown page or the C# options contract |

Consuming applications can feed this XML documentation to Swagger. Keep it
aligned with the actual HTTP contract and inspect the generated document when
changing that integration.

## Commits and releases

GitVersion computes package versions from commit subjects on `main`; no
release is tagged, so every commit in history counts. With the rules in
`GitVersion.yml`:

| Subject | Version bump |
| --- | --- |
| `feat:` or `feat(scope):` | Minor |
| `fix:` or `perf:` | Patch |
| `!` after the type or scope, or a `BREAKING CHANGE:` footer | Major |
| Any other type | Patch, the `main` branch default |
| `skip:` | None |

The legacy `breaking:` and `feature:` prefixes remain in `GitVersion.yml` only
so that old commits compute the same version. Do not use them for new commits.

Scopes: the affected package, `deps`, `ci`, `agents`.

The squash message on merge decides the bump, so check it against this table.

## Pull requests

Name the rollout section `## Migration`. It covers breaking contracts,
dependency changes, and configuration steps consuming applications must take.

## Domain checks

- Public contract changes document the consumer-visible behavior and how
  consumers migrate.
- Distinguish local build or pack commands from publishing commands, so a
  reader verifying a change does not release a package.
