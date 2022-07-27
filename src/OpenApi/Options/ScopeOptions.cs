using JetBrains.Annotations;

namespace OpenApi.Options;

[PublicAPI]
public class ScopeOptions
{
    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;
}