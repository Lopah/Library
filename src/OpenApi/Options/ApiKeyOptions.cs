using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace OpenApi.Options;

[PublicAPI]
public class ApiKeyOptions
{
    [Required]
    public string HeaderName { get; init; } = null!;

    public string? Description { get; init; }
}