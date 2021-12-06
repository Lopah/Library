using System.ComponentModel.DataAnnotations;

namespace OpenApi.Options;

public class ApiKeyOptions
{
    [Required] public string HeaderName { get; init; }

    public string? Description { get; init; }
}
