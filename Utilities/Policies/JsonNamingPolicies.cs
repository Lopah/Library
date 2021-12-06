using System.Text.Json;

namespace Utilities.Policies;

public static class JsonNamingPolicies
{
    public static JsonNamingPolicy UpperSnakeCase { get; } = new JsonUpperSnakeCaseNamingPolicy();
}
