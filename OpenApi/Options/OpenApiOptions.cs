using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Models;

namespace OpenApi.Options;

public class OpenApiOptions
{
    internal const string DefaultRouteTemplate = "api/contracts/{documentName}/openapi.json";

    public bool DisableSwagger { get; init; }

    [Required] public OpenApiInfoOptions OpenApiInfo { get; init; }

    [Required] public SecurityOptions Security { get; init; }

    public IEnumerable<OpenApiServer> Servers { get; init; }

    [Required] public string RoutePrefix { get; init; }

    public string RouteTemplate { get; init; } = DefaultRouteTemplate;

    public DefaultApiVersionOptions DefaultApiVersion { get; init; }
}
