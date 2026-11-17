# Test changes

For agents adding or changing tests under `tests/`.

Use the existing xUnit and FluentAssertions conventions. Choose the API,
Utilities, or Validation test project according to the owning behavior;
OpenAPI integration tests are in the API test project.

## Prove consumer behavior

- Name new tests after the condition and expected behavior.
- For a regression, add a case that fails with the old behavior and passes
  with the fix. Include the edge case that caused the failure.
- Test serialization through JSON values, validation through validation
  results, and middleware or Swagger registration through the in-process
  `TestServer` where appropriate.
- Keep test configuration self-contained. These library tests should not need
  production credentials or live external services.

## Run focused tests

From the repository root:

```bash
dotnet test tests/Lopah.Library.Api.Tests/Lopah.Library.Api.Tests.csproj --configuration Release --filter FullyQualifiedName~SlugifyParameterTransformerTests
```

Substitute the project and filter for the behavior being changed. Use
`--no-build` only after building the current code in the same configuration.
Run the full solution tests before handing off changes that affect shared
behavior.
