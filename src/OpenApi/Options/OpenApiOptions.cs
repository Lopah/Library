using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.OpenApi.Models;

namespace OpenApi.Options;

[PublicAPI]
public class OpenApiOptions
{
    internal const string DefaultRouteTemplate = "api/contracts/{documentName}/openapi.json";

    public bool DisableSwagger { get; init; }

    [Required]
    public OpenApiInfoOptions OpenApiInfo { get; init; } = null!;

    [Required]
    public SecurityOptions Security { get; init; } = null!;

    public IEnumerable<OpenApiServer>? Servers { get; init; }

    [Required]
    public string RoutePrefix { get; init; } = null!;

    public string RouteTemplate { get; init; } = DefaultRouteTemplate;

    public DefaultApiVersionOptions DefaultApiVersion { get; init; } = null!;
}