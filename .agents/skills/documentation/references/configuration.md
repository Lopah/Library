# Configuration documentation standard

For Forgejo workflows, YAML, JSON configuration, and project files in Lopah
Library.

Give workflow jobs and steps names that state their outcome. Comment
non-obvious concurrency choices, conditions, permissions, or ordering with the
failure they prevent.

Explain settings that affect consumers or publishing in the README. Keep the
workflow and project files authoritative for exact versions and conditions;
link to them rather than duplicating long configuration blocks.

JSON does not support comments. Document JSON options in the owning Markdown
page or C# options contract. For XML project files and YAML, add comments only
where the reason is not evident from the setting itself.

Show secret names and configuration mechanisms, never actual secret values.
Distinguish local build or pack commands from publishing commands so a reader
does not release packages while trying to verify a change.
