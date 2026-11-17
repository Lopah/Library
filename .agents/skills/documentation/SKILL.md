---
name: documentation
description:
  Use when writing or reviewing code comments, C# XML documentation,
  configuration comments, Markdown, commit messages, or pull-request
  descriptions in Lopah Library. Defines what to document and where it belongs.
---

# Documentation standard

For agents and contributors writing comments, documentation, commits, or pull
requests (PRs) in Lopah Library.

## Document why, not what

Record the knowledge that code cannot show: the reason for a decision, a
consumer constraint, an external package quirk, or a non-obvious consequence.
Prefer a better name or smaller function when that removes the need for a
comment.

XML documentation has a separate purpose: describe the public contract a
consumer relies on, including constraints not expressed by the signature.

## Put knowledge in one place

| Knowledge | Location |
| --- | --- |
| Reason for a line or block | Comment beside the code |
| Public type or member contract | C# XML documentation |
| Consumer setup, configuration, or migration | README or a focused page in `docs/` |
| Cross-library design decision | ADR in `docs/adr/`, when needed |
| Release-facing behavior change | `CHANGELOG.md` |
| Why this change is needed and its risks | Commit body and PR description |
| Follow-up work | Forgejo issue, referenced by `TODO(#123)` |

Link to the owner rather than copying its explanation. Keep change history in
Git, not comments. Remove commented-out code when changing the affected area;
avoid unrelated cleanup.

## Load the relevant reference

| Writing | Reference |
| --- | --- |
| C# comments, XML documentation, test names | [csharp.md](./references/csharp.md) |
| README, Markdown pages, AGENTS.md, skills | [markdown.md](./references/markdown.md) |
| Forgejo YAML and configuration comments | [configuration.md](./references/configuration.md) |
| Commit messages and PR descriptions | [commits.md](./references/commits.md) |

## Review before finishing

- Every new comment explains a reason, constraint, or external fact.
- Public contract changes document consumer-visible behavior and migration.
- Changed setup instructions match the relevant project or workflow.
- Relative links resolve and command examples use this repository's paths.
- Commit wording accounts for the version rules in `GitVersion.yml`.
