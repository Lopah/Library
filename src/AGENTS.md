# Library changes

For agents changing code under `src/`.

## Preserve package boundaries and contracts

Choose the owning library before editing. Check its `.csproj` and consuming
projects before adding a dependency; keep general utilities independent of
ASP.NET Core and Swagger concerns.

Treat public signatures, namespaces, configuration keys, default values,
exception behavior, validation rules, and JSON output as consumer contracts.
Retain compatibility unless the task calls for a breaking change. Describe the
migration and add tests for the affected consumer behavior.

Use the target framework and language settings in the project files. A .NET 11
upgrade is planned; load the repo-local
[C# 15 skill](../.agents/skills/csharp-15/SKILL.md) for that upgrade and for code
targeting `net11.0`. Keep older-targeted projects compatible with their current
compiler. The skill does not authorize changing the target framework or
enabling preview features outside the requested work.

Preserve the consuming application's serialization behavior when changing JSON
configuration or framework dependencies.

## Verify OpenAPI changes through a consuming application

When changing `src/OpenApi/` or API documentation registration in `src/Api/`,
extend `tests/Lopah.Library.Api.Tests/OpenApiIntegrationTests.cs`, creating it
if absent. Use an in-process ASP.NET Core application to check both document
generation and HTTP behavior without a separately running server.

Assert the generated document or UI link affected by the change, not only the
filter's internal state. Cover relevant version groups, `UsePathBase`, route
prefixes, authorization metadata, and disabled documentation behavior.

Check referenced package versions before adapting Swashbuckle examples:
namespace and schema interfaces differ across OpenAPI package generations.
Keep documented schema formats consistent with the JSON converters and
validation attributes that produce the actual HTTP contract.
