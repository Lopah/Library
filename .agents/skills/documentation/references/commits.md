# Commit and pull-request standard

For commit messages, pull-request (PR) titles, and descriptions in Lopah Library.

## Choose version-aware wording

Read root `GitVersion.yml` before selecting a subject. This repository has
custom commit-message increment patterns; the portal's `feat` convention is
not a substitute for them.

For a version-affecting change, use the prefix that matches the intended
increment in that configuration. For example, the current feature pattern
matches `feature: ...`, and the major pattern matches `breaking: ...`.
Recheck the file rather than treating these examples as permanent rules.
For other changes, follow recent repository conventions and avoid implying an
unintended version bump.

Write an imperative subject naming the outcome. Explain why the change is
needed, its consumer-visible behavior, and any compatibility or migration
risks in the body. A file list is not a substitute for that explanation.

Use `Closes #123` only when the change completes the issue; otherwise use
`Refs #123`. Explicitly describe incompatible changes and how consumers adapt.

## Describe the PR

Use this outline, omitting sections that do not apply:

```markdown
Refs #123

## Why

Problem or requirement.

## What changes

Behavior visible to consuming applications or maintainers.

## Migration

Breaking contracts, dependency changes, or configuration steps.

## Testing

Commands run and their results, including anything not verified.
```

Retrieve issue requirements and discussions using the repo-local
[Forgejo workflow](../../forgejo/SKILL.md) before claiming alignment.
If a requested merge uses squashing, apply the same version-aware wording to
the final squash message.
