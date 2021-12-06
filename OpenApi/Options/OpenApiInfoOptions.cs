using System.ComponentModel.DataAnnotations;

namespace OpenApi.Options;

public class OpenApiInfoOptions
{
    /// <summary>
    ///     Version displaed in OpenAPI scpeification. If null, it uses [ApiVersion] attribute
    /// </summary>
    public string? Version { get; init; }

    [Required] public string Title { get; init; }

    [Required] public string Description { get; init; }

    [Required] public ContactOptions Contact { get; set; }

    [Required] public LicenceOptions Licence { get; init; }
}
