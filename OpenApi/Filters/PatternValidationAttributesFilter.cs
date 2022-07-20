using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Validation;

namespace OpenApi.Filters;

public class PatternValidationAttributesFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is null)
        {
            throw new ArgumentNullException(nameof(schema));
        }

        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.ParameterInfo != null)
        {
            ApplyApiCustomAttributes(schema, context.ParameterInfo.GetCustomAttributes());
        }

        if (context.MemberInfo != null)
        {
            ApplyApiCustomAttributes(schema, context.MemberInfo.GetInlineAndMetadataAttributes());
        }
    }

    private static void ApplyApiCustomAttributes(OpenApiSchema schema, IEnumerable<object> customAttributes)
    {
        foreach (var attribute in customAttributes.OfType<IPatternValidationAttribute>())
        {
            schema.Pattern = attribute.Pattern;
        }
    }
}