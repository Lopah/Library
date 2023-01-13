using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Api.Options;

[PublicAPI]
public class CorsOptions
{
    [Required]
    [MinLength(1)]
    public IReadOnlyCollection<string> Methods { get; init; } = null!;

    public ICollection<string> Origins { get; init; } = new List<string>();

    public ICollection<string> ExposedHeaders { get; init; } = new List<string>();
}