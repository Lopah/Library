using System.ComponentModel.DataAnnotations;

namespace OpenApi.Options;

public class ClientCredentialOptions
{
    [Required]
    public Uri TokenUrl { get; init; } = null!;

    [Required]
    public Uri RefreshUrl { get; init; } = null!;

    [Required]
    public IEnumerable<ScopeOptions> Scopes { get; init; } = null!;
}