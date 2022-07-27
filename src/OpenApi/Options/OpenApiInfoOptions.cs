using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace OpenApi.Options;

[PublicAPI]
public class OpenApiInfoOptions
{
    /// <summary>
    ///     Version displaed in OpenAPI scpeification. If null, it uses [ApiVersion] attribute
    /// </summary>
    public string? Version { get; init; }

    [Required]
    public string Title { get; init; } = null!;

    [Required]
    public string Description { get; init; } = null!;

    [Required]
    public ContactOptions Contact { get; set; } = null!;

    [Required]
    public LicenceOptions Licence { get; init; } = null!;
}