using System.ComponentModel.DataAnnotations;

namespace OpenApi.Options;

public class AuthorizationCodeOptions
{
    [Required] public Uri TokenUrl { get; init; }

    [Required] public Uri AuthorizationUrl { get; init; }

    [Required] public IEnumerable<ScopeOptions> Scopes { get; init; }
}
