using System.ComponentModel.DataAnnotations;

namespace OpenApi.Options;

public class ClientCredentialOptions
{
    [Required] public Uri TokenUrl { get; init; }

    [Required] public Uri RefreshUrl { get; init; }

    [Required] public IEnumerable<ScopeOptions> Scopes { get; init; }
}
