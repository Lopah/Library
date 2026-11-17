# Lopah Library

Shared API, OpenAPI, validation, and utility libraries targeting .NET 11.

## Development

The SDK is pinned to **11.0.100** in `global.json`, with SDK roll-forward disabled.
Forgejo Actions installs that same version from `global.json`.

Local tools are pinned in `.config/dotnet-tools.json`:

| Tool | Version |
| --- | --- |
| GitVersion | 6.8.2 |
| ReportGenerator | 5.5.11 |

```sh
dotnet tool restore
dotnet restore Lopah.Library.sln
dotnet build Lopah.Library.sln --configuration Release
dotnet test Lopah.Library.sln --configuration Release
dotnet pack Lopah.Library.sln --configuration Release --no-build --output artifacts/packages
```

To collect coverage and generate an HTML report:

```sh
dotnet test Lopah.Library.sln --configuration Release --no-build --collect "XPlat Code Coverage" --results-directory artifacts/test-results
dotnet tool run reportgenerator -- "-reports:artifacts/test-results/**/coverage.cobertura.xml" "-targetdir:artifacts/coverage"
```

The HTML report is written to `artifacts/coverage/index.html`.

The GitVersion 6 configuration retains the Mainline calculation strategy, the
existing `main` branch mapping, and the repository's commit-message increment
rules. Inspect the calculated version with:

```sh
dotnet tool run dotnet-gitversion -- /nofetch /output json
```

## Forgejo Actions

`.forgejo/workflows/ci.yml` uses plain .NET CLI commands to restore, version,
build, test, collect coverage, and pack all four libraries. It runs for branch
pushes and pull requests, and uploads packages, test results, and coverage as
workflow artifacts.

Pushes to `main`, `master`, `release/*`, and `hotfix/*` also publish packages to
nuget.org and create a Forgejo release with the packages attached. Configure the
repository secret `NUGET_API_KEY` for nuget.org publishing. Forgejo releases use
the workflow's automatic repository token.

The workflow requires a Docker-capable Forgejo runner with the `ubuntu-22.04` label,
matching the shared runners used by the other repositories.
It uses a Node 24 Debian container so JavaScript actions can execute, and
installs the SDK pinned in `global.json`. Package versions use GitVersion's
`MajorMinorPatch`; the informational version records the GitVersion metadata.

## Versioned Swagger configuration

Swagger uses Swashbuckle 10, OpenAPI 3.1, and `Asp.Versioning.Mvc.ApiExplorer`.
API versions are read from URL segments, with one document per API version.

Register controller and documentation services in the consuming application:

```csharp
using Api.Extensions;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using OpenApi.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options => options.AddApiJsonOptions());
builder.Services.AddVersionedOpenApi(builder.Configuration);

var app = builder.Build();
app.UseVersionedOpenApi(
    app.Services.GetRequiredService<IApiVersionDescriptionProvider>(),
    app.Services.GetRequiredService<IOptions<OpenApiOptions>>());
app.MapControllers();
app.Run();
```

Example application configuration:

```json
{
  "OpenApiOptions": {
    "OpenApiInfo": {
      "Title": "Example API",
      "Description": "Example API contracts",
      "Contact": {
        "Name": "API team",
        "Email": "api@example.com",
        "Url": "https://example.com"
      },
      "Licence": {
        "Name": "MIT",
        "Url": "https://opensource.org/licenses/MIT"
      }
    },
    "Security": {
      "ApiKey": {
        "HeaderName": "X-Api-Key",
        "Description": "API key"
      }
    },
    "DefaultApiVersion": { "Major": 1, "Minor": 0 },
    "RoutePrefix": "swagger",
    "RouteTemplate": "api/contracts/{documentName}/openapi.json"
  }
}
```

Controllers use attributes from `Asp.Versioning`, such as `[ApiVersion("1.0")]`,
and routes such as `[Route("api/v{version:apiVersion}/[controller]")]`.
Documentation endpoints appear at `/api/contracts/v1/openapi.json`; the UI
defaults to `/swagger`. Set `RoutePrefix` to an empty string to serve it at the
application root. UI document links also work under `UsePathBase`.

OAuth flows can be configured instead of `Security.ApiKey`, using the existing
`AuthorizationCode`, `ClientCredentials`, or `Implicit` options. Authorization
code flows enable PKCE in the UI. Security requirements respect `[Authorize]`
and `[AllowAnonymous]`. Set `DisableSwagger` to `true` to disable documentation
while retaining API versioning.

Enable `GenerateDocumentationFile` in the consuming application's project to
include XML comments in the generated documents.

## Migration notes

- All library and test projects now target `net11.0`. Consuming applications must
  upgrade to .NET 11 before updating these packages.
- Automation runs through Forgejo Actions and the .NET CLI.
- ASP.NET Core APIs come from the `Microsoft.AspNetCore.App` shared framework.
  JSON configuration uses System.Text.Json; the legacy Newtonsoft-based MVC
  formatter and unused Macross JSON package have been removed.
- Replace `Microsoft.AspNetCore.Mvc.Versioning` imports with `Asp.Versioning`
  and versioned API explorer imports with `Asp.Versioning.ApiExplorer`.
- OpenAPI model types now live in `Microsoft.OpenApi`. Custom schema filters
  accept `IOpenApiSchema` and mutate concrete `OpenApiSchema` instances;
  schema types use `JsonSchemaType` instead of strings.
- Date-only converters are documented as `date`, timestamp converters as
  `date-time`, and ETag response headers as strings.
