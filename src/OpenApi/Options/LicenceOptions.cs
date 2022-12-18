using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace OpenApi.Options;

[PublicAPI]
public class LicenceOptions
{
    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public Uri Url { get; init; } = null!;
}