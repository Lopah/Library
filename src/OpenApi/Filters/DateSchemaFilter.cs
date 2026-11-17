using System;
using System.Reflection;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Utilities.Converters;

namespace OpenApi.Filters;

[PublicAPI]
public class DataSchemaFilter : ISchemaFilter
{
    private const string DateFormat = "date";

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

        var converterAttribute = context.MemberInfo?.GetCustomAttribute<JsonConverterAttribute>();

        if (schema is not OpenApiSchema openApiSchema || converterAttribute == null)
        {
            return;
        }

        if (converterAttribute.ConverterType == typeof(DateConverter) ||
            converterAttribute.ConverterType == typeof(DateOnlyConverter))
        {
            openApiSchema.Format = DateFormat;
        }
        else if (converterAttribute.ConverterType == typeof(DateTimeConverter))
        {
            openApiSchema.Format = "date-time";
        }
    }
}
