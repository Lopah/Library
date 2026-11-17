# C# documentation standard

For comments and XML documentation in `src/` and `tests/`. Apply the
[documentation rule](../SKILL.md): explain constraints and contracts.

## Code comments

Put a short comment above code that a careful reader might otherwise change
incorrectly. Explain non-obvious ordering, compatibility workarounds, or
framework behavior. Include the reason for analyzer or ReSharper suppressions.

For example, a comment above a relative Swagger URL calculation can explain why
an absolute path would break consumers hosted under `UsePathBase`.

Prefer names and extracted helpers over comments narrating control flow.
Record a bug's history in the commit or issue, not beside the fixed code.

## XML documentation

Describe what callers may rely on and must provide:

- null handling and meaningful thrown exceptions;
- units, ranges, defaults, and effects of options;
- serialization formats, culture, and time-zone assumptions;
- validation semantics that are not obvious from an attribute's name;
- side effects, ownership, and disposal obligations.

Write docs for new or changed public contracts when these details matter.
Avoid summaries and parameter tags that only repeat the signature.

Use `<summary>` for the contract, `<remarks>` for longer rationale, and
`<see cref="..."/>` for code references. Document exceptions callers are
expected to handle.

Swagger can consume a consuming application's XML documentation when its
documentation file is generated and included. Keep those comments aligned with
the actual HTTP contract; inspect the generated document when changing that
integration.

## Tests and logging

Name tests after the condition and expected behavior. Comment fixture values
only when the reason for the value matters, such as a culture-sensitive input
or a boundary date.

Log messages should state the event and include the identifiers needed to
investigate it. Keep credentials and sensitive request data out of messages.
