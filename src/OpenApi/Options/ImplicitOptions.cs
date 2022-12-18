using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace OpenApi.Options;

[PublicAPI]
public class ImplicitOptions
{
    [Required]
    public Uri AuthorizationUrl { get; init; } = null!;

    [Required]
    public IEnumerable<ScopeOptions> Scopes { get; init; } = null!;
}