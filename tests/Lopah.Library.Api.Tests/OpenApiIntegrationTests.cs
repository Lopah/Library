using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Api.Extensions;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenApi.Attributes;
using OpenApi.Options;
using Utilities.Converters;
using Validation;
using Xunit;

namespace Lopah.Library.Api.Tests;

public class OpenApiIntegrationTests
{
    [Fact]
    public async Task ApiJsonOptions_ShouldUseSystemTextJsonConventionsInControllerResponses()
    {
        await using var app = await CreateApplication("swagger");
        using var response = JsonDocument.Parse(
            await app.GetTestClient().GetStringAsync("/library/api/v2/contracts/json"));

        response.RootElement.GetProperty("email").GetString().Should().Be("test@example.com");
        response.RootElement.GetProperty("date").GetString().Should().Be("2026-10-03");
        response.RootElement.GetProperty("status").GetString().Should().Be("READY_TO_SEND");
        response.RootElement.TryGetProperty("timestamp", out _).Should().BeFalse();
    }

    [Theory]
    [InlineData("", "api/contracts/v2/openapi.json")]
    [InlineData("swagger", "../api/contracts/v2/openapi.json")]
    [InlineData("docs/swagger", "../../api/contracts/v2/openapi.json")]
    public async Task VersionedSwagger_ShouldGenerateDocumentsAndPathBaseRelativeUiLinks(
        string routePrefix, string documentUrl)
    {
        await using var app = await CreateApplication(routePrefix);
        var client = app.GetTestClient();

        using var latest = JsonDocument.Parse(await client.GetStringAsync("/library/api/contracts/v2/openapi.json"));
        latest.RootElement.GetProperty("openapi").GetString().Should().StartWith("3.1.");
        latest.RootElement.GetProperty("info").GetProperty("version").GetString().Should().Be("2.0");
        latest.RootElement.GetProperty("info").GetProperty("title").GetString().Should().Be("Test API");
        latest.RootElement.GetProperty("servers")[0].GetProperty("url").GetString().Should().Be("https://api.example.com");

        var paths = latest.RootElement.GetProperty("paths");
        paths.TryGetProperty("/api/v2/contracts", out var contractPath).Should().BeTrue();
        var contract = contractPath.GetProperty("get");
        contract.GetProperty("security")[0].TryGetProperty("psk", out _).Should().BeTrue();
        var headers = contract.GetProperty("responses").GetProperty("200").GetProperty("headers");
        headers.GetProperty("ETag").GetProperty("schema").GetProperty("type").GetString().Should().Be("string");
        headers.GetProperty("Vary").GetProperty("schema").GetProperty("type").GetString().Should().Be("string");

        var file = paths.GetProperty("/api/v2/contracts/file").GetProperty("get");
        file.TryGetProperty("security", out _).Should().BeFalse();
        var fileContent = file.GetProperty("responses").GetProperty("200").GetProperty("content");
        fileContent.GetProperty("application/octet-stream").GetProperty("schema").GetProperty("format")
            .GetString().Should().Be("binary");

        var properties = latest.RootElement.GetProperty("components").GetProperty("schemas")
            .GetProperty(nameof(ContractResponse)).GetProperty("properties");
        properties.GetProperty("email").GetProperty("pattern").GetString().Should().NotBeNullOrEmpty();
        properties.GetProperty("date").GetProperty("format").GetString().Should().Be("date");
        properties.GetProperty("timestamp").GetProperty("format").GetString().Should().Be("date-time");

        using var deprecated = JsonDocument.Parse(await client.GetStringAsync("/library/api/contracts/v1/openapi.json"));
        deprecated.RootElement.GetProperty("info").GetProperty("description").GetString()
            .Should().Contain("deprecated");

        var uiPath = string.IsNullOrEmpty(routePrefix) ? "index.html" : $"{routePrefix}/index.html";
        var ui = await client.GetStringAsync($"/library/{uiPath}");
        ui.Should().Contain("Swagger UI");
        var uiScript = await client.GetStringAsync($"/library/{uiPath.Replace("index.html", "index.js")}");
        uiScript.Should().Contain(documentUrl);
        var resolvedDocumentUrl = new Uri(new Uri(client.BaseAddress!, $"/library/{uiPath}"), documentUrl);
        (await client.GetAsync(resolvedDocumentUrl)).IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task OAuthSwagger_ShouldDescribeAuthorizationCodeFlowAndSecureOnlyAuthorizedOperations()
    {
        await using var app = await CreateApplication("swagger", useOAuth: true);
        using var document = JsonDocument.Parse(
            await app.GetTestClient().GetStringAsync("/library/api/contracts/v2/openapi.json"));

        var flow = document.RootElement.GetProperty("components").GetProperty("securitySchemes")
            .GetProperty("oauth2").GetProperty("flows").GetProperty("authorizationCode");
        flow.GetProperty("authorizationUrl").GetString().Should().Be("https://identity.example.com/authorize");
        flow.GetProperty("tokenUrl").GetString().Should().Be("https://identity.example.com/token");
        flow.GetProperty("scopes").GetProperty("contracts.read").GetString().Should().Be("Read contracts");
        document.RootElement.GetProperty("paths").GetProperty("/api/v2/contracts").GetProperty("get")
            .GetProperty("security")[0].TryGetProperty("oauth2", out _).Should().BeTrue();
        document.RootElement.GetProperty("paths").GetProperty("/api/v2/contracts/file").GetProperty("get")
            .TryGetProperty("security", out _).Should().BeFalse();
    }

    [Fact]
    public async Task DisabledSwagger_ShouldKeepApiVersioningAndSkipDocumentationEndpoints()
    {
        await using var app = await CreateApplication("swagger", disableSwagger: true);
        app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions.Should().HaveCount(2);
        var response = await app.GetTestClient().GetAsync("/library/api/contracts/v2/openapi.json");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    private static async Task<WebApplication> CreateApplication(
        string routePrefix, bool disableSwagger = false, bool useOAuth = false)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        var configuration = new Dictionary<string, string?>
        {
            ["OpenApiOptions:OpenApiInfo:Title"] = "Test API",
            ["OpenApiOptions:OpenApiInfo:Description"] = "Integration test contracts",
            ["OpenApiOptions:OpenApiInfo:Contact:Name"] = "Test",
            ["OpenApiOptions:OpenApiInfo:Contact:Email"] = "test@example.com",
            ["OpenApiOptions:OpenApiInfo:Contact:Url"] = "https://example.com",
            ["OpenApiOptions:OpenApiInfo:Licence:Name"] = "MIT",
            ["OpenApiOptions:OpenApiInfo:Licence:Url"] = "https://example.com/license",
            ["OpenApiOptions:Security:ApiKey:HeaderName"] = "X-Api-Key",
            ["OpenApiOptions:Security:ApiKey:Description"] = "Test API key",
            ["OpenApiOptions:RoutePrefix"] = routePrefix,
            ["OpenApiOptions:DisableSwagger"] = disableSwagger.ToString(),
            ["OpenApiOptions:Servers:0:Url"] = "https://api.example.com"
        };
        if (useOAuth)
        {
            configuration.Remove("OpenApiOptions:Security:ApiKey:HeaderName");
            configuration.Remove("OpenApiOptions:Security:ApiKey:Description");
            configuration["OpenApiOptions:Security:AuthorizationCode:AuthorizationUrl"] = "https://identity.example.com/authorize";
            configuration["OpenApiOptions:Security:AuthorizationCode:TokenUrl"] = "https://identity.example.com/token";
            configuration["OpenApiOptions:Security:AuthorizationCode:Scopes:0:Name"] = "contracts.read";
            configuration["OpenApiOptions:Security:AuthorizationCode:Scopes:0:Description"] = "Read contracts";
        }

        builder.Configuration.AddInMemoryCollection(configuration);
        builder.Services.AddControllers().AddApplicationPart(typeof(ContractsController).Assembly)
            .AddJsonOptions(options => options.AddApiJsonOptions());
        builder.Services.AddVersionedOpenApi(builder.Configuration);

        var app = builder.Build();
        app.UsePathBase("/library");
        app.UseVersionedOpenApi(
            app.Services.GetRequiredService<IApiVersionDescriptionProvider>(),
            app.Services.GetRequiredService<IOptions<OpenApiOptions>>());
        app.MapControllers();
        await app.StartAsync();
        return app;
    }
}

[ApiController]
[ApiVersion("1.0", Deprecated = true)]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/contracts")]
public class ContractsController : ControllerBase
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ContractResponse), 200)]
    [SwaggerETagResponseHeader]
    [SwaggerVaryResponseHeader]
    public ActionResult<ContractResponse> Get() => new ContractResponse();

    [HttpGet("file")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Stream), 200)]
    public IActionResult GetFile() => File(Array.Empty<byte>(), "application/octet-stream");

    [HttpGet("json")]
    [AllowAnonymous]
    public ActionResult<ContractResponse> GetJson() => new ContractResponse { Date = new DateTime(2026, 10, 3) };
}

public class ContractResponse
{
    [EmailAddressValidation]
    public string Email { get; set; } = "test@example.com";

    [JsonConverter(typeof(DateConverter))]
    public DateTime Date { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTimeOffset? Timestamp { get; set; }

    public ContractStatus Status { get; set; } = ContractStatus.ReadyToSend;
}

public enum ContractStatus
{
    ReadyToSend
}
