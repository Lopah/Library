using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Validation;

namespace OpenApi.Filters;

[PublicAPI]
public class PatternValidationAttributesFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is null)
        {
            throw new ArgumentNullException(nameof(schema));
        }

        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (schema is not OpenApiSchema openApiSchema)
        {
            return;
        }

        if (context.ParameterInfo != null)
        {
            ApplyApiCustomAttributes(openApiSchema, context.ParameterInfo.GetCustomAttributes());
        }

        if (context.MemberInfo != null)
        {
            ApplyApiCustomAttributes(openApiSchema, context.MemberInfo.GetInlineAndMetadataAttributes());
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
