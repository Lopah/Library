using System.Text.Json;
using Humanizer;

namespace Utilities.Policies;

internal sealed class JsonUpperSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.Underscore().ToUpperInvariant();
    }
}