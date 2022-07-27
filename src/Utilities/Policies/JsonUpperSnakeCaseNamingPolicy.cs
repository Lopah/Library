using System.Text.Json;
using Humanizer;
using JetBrains.Annotations;

namespace Utilities.Policies;

[PublicAPI]
internal sealed class JsonUpperSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.Underscore().ToUpperInvariant();
    }
}