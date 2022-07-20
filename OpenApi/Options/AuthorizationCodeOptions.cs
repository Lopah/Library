using System.ComponentModel.DataAnnotations;

namespace OpenApi.Options;

public class AuthorizationCodeOptions
{
    [Required]
    public Uri TokenUrl { get; init; } = null!;

    [Required]
    public Uri AuthorizationUrl { get; init; } = null!;

    [Required]
    public IEnumerable<ScopeOptions> Scopes { get; init; } = null!;
}