using System.ComponentModel.DataAnnotations;

namespace Api.Options;

public class CorsOptions
{
    [Required]
    [MinLength(1)]
    public IReadOnlyCollection<string> Methods { get; init; }

    public ICollection<string> Origins { get; init; }

    public ICollection<string> ExposedHeaders { get; init; }
}