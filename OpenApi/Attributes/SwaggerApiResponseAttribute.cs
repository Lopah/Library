using System.Reflection;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Annotations;

namespace OpenApi.Attributes;

[PublicAPI]
public class SwaggerApiResponseAttribute : SwaggerResponseAttribute
{
    public SwaggerApiResponseAttribute(string operationId, int statusCode, Type resourceType,
        string? defaultDescription = null, Type? type = null)
        : base(statusCode, GetDescription(operationId, resourceType, statusCode, defaultDescription), type)
    {
    }

    private static string? GetDescription(string operationId, Type resourceType, int statusCode,
        string? defaultDescription)
    {
        if (resourceType is null)
        {
            throw new ArgumentNullException(nameof(resourceType));
        }

        var description = defaultDescription;
        var descriptionProperty = resourceType.GetProperty($"{operationId}{statusCode}{nameof(Description)}",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

        if (descriptionProperty != null && descriptionProperty.PropertyType == typeof(string))
        {
            description = descriptionProperty.GetValue(null, null) as string;
        }

        return description;
    }
}