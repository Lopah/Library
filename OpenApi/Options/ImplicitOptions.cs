using System.ComponentModel.DataAnnotations;

namespace OpenApi.Options;

public class ImplicitOptions
{
    [Required]
    public Uri AuthorizationUrl { get; init; }

    [Required]
    public IEnumerable<ScopeOptions> Scopes { get; init; }
}