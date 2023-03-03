using System.Text.Json;
using JetBrains.Annotations;

namespace Utilities.Policies;

[PublicAPI]
public static class JsonNamingPolicies
{
    public static JsonNamingPolicy UpperSnakeCase { get; } = new JsonUpperSnakeCaseNamingPolicy();
    public static JsonNamingPolicy SnakeCase { get; } = new JsonSnakeCaseNamingPolicy();
}