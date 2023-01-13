using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace OpenApi.Options;

[PublicAPI]
public class ClientCredentialOptions
{
    [Required]
    public Uri TokenUrl { get; init; } = null!;

    [Required]
    public Uri RefreshUrl { get; init; } = null!;

    [Required]
    public IEnumerable<ScopeOptions> Scopes { get; init; } = null!;
}