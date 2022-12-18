using System.Reflection;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Annotations;

namespace OpenApi.Attributes;

[PublicAPI]
public class SwaggerApiSchemaAttribute : SwaggerSchemaAttribute
{
    public SwaggerApiSchemaAttribute(string defaultDescription, Type resourceType, string resourceName)
    {
        if (resourceType is null)
        {
            throw new ArgumentNullException(nameof(resourceType));
        }

        var description = defaultDescription;

        var property = resourceType
            .GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

        if (property != null && property.PropertyType == typeof(string))
        {
            description = property.GetValue(null, null) as string;
        }

        Description = description;
    }
}