# Markdown documentation standard

For READMEs, guides, architecture decision records (ADRs), AGENTS.md, and
skills in Lopah Library.

## Choose the owner

- Keep consumer setup and migration guidance in the README; use a focused
  `docs/<topic>.md` page when a topic outgrows it.
- Keep release-facing changes in `CHANGELOG.md`, following its existing format.
- Keep AGENTS.md a map and put multi-step agent workflows in
  `.agents/skills/<name>/SKILL.md`.
- For a cross-library decision, create an ADR in `docs/adr/` when needed.
  State the decision, status, rationale, rejected alternative, and conditions
  for revisiting it. Supersede an accepted decision with a new ADR.

## Write for the task

Open with the reader and task. Put the main action before background, use
headings that name an action or question, and keep sentences short and active.
Use numbered lists for ordered steps and tables for options.

Make commands copy-pasteable from a stated working directory. Show required
configuration without real credentials. State the expected result and explain
non-obvious prerequisites.

Expand acronyms on first use. Link to existing explanations rather than
repeating them. Verify relative links from the file containing the link.

## Agent instructions

Write instructions with an observable completion condition. A skill's
description starts with "Use when" and states the tasks that should trigger it.
Move branch-specific reference material behind links so unrelated tasks do not
need to load it.
