using System.ComponentModel.DataAnnotations;

namespace OpenApi.Options;

public class LicenceOptions
{
    [Required]
    public string Name { get; init; }

    [Required]
    public Uri Url { get; init; }
}