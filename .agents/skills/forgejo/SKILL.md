---
name: forgejo
description:
  Use when reading or updating Lopah Library's Forgejo issues, pull requests,
  review discussions, or linked requirements, including git.lopah.eu links.
  Use the authenticated tea CLI to retrieve requirements and manage tracker work.
---

# Forgejo tracker workflow

For agents working in Lopah Library, use `tea` to access the private Forgejo
repository `lopah/Library` at `https://git.lopah.eu`. Use the saved login
explicitly because it may not be the default. Use authenticated CLI requests
rather than unauthenticated web requests or GitHub's `gh` CLI.

## Retrieve requirements and discussions

1. Confirm the CLI and saved login are available:

   ```bash
   tea --version
   tea login list
   ```

   Use the login whose URL is `https://git.lopah.eu`. The examples use the login
   name `git.lopah.eu`; substitute its actual name if it differs. If the CLI is
   unavailable, report that prerequisite. If no matching login exists, ask the
   requester to authenticate with `tea login add`. Keep credentials in the CLI's
   credential store, not repository files or chat output.

2. Read the requested issue or pull request (PR), including comments. Replace
   the example numbers with the requested records:

   ```bash
   tea issues --login git.lopah.eu --repo lopah/Library --comments --fields index,url,title,body,state,comments --output json 123
   tea pulls --login git.lopah.eu --repo lopah/Library --comments --fields index,url,title,body,state,base,base-commit,head,comments --output json 456
   tea pulls review-comments --login git.lopah.eu --repo lopah/Library --output json 456
   ```

   For discovery, use `tea issues` or `tea pulls` with the same explicit login
   and repository flags. Their default lists show open records; select
   `--state all` when looking for closed work and paginate when needed.

3. Follow issue references in the PR description and commit messages. For a
   code review, confirm the PR's base branch and head commit match the local
   comparison. Proceed once the relevant description, issue requirements,
   comments, and inline review comments have been retrieved. If access fails,
   report the failure and request the missing text rather than guessing.

   These commands target this repository. For an explicitly linked record in
   another repository, select that link's owner and repository before reading.

## Update tracker records

Use `tea <command> --help` to confirm the installed CLI's flags before editing,
commenting, submitting a review, resolving discussions, or merging. Perform
only the tracker action requested by the user and return the affected issue or
PR URL. Reading or reviewing locally does not authorize posting or merging.
