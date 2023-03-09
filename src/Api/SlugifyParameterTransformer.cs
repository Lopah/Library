using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Api;

public partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    /// <summary>
    /// Transforms the specified route value to a string for inclusion in a URI.
    /// </summary>
    /// <param name="value">The route value to transform.</param>
    /// <returns>The transformed value.</returns>
    public string? TransformOutbound(object? value)
    {
        if (value is null)
        {
            return null;
        }

        var stringifiedValue = value.ToString();

        return string.IsNullOrEmpty(stringifiedValue) ? null : MyRegex().Replace(stringifiedValue, "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}