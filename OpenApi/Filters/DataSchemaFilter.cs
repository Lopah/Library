using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Utilities.Converters;

namespace OpenApi.Filters;

public class DataSchemaFilter : ISchemaFilter
{
    private const string DateFormat = "date";

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is null) throw new ArgumentNullException(nameof(schema));

        if (context is null) throw new ArgumentNullException(nameof(context));

        var converterAttribute = context.MemberInfo?.GetCustomAttribute<JsonConverterAttribute>();

        if (converterAttribute != null && converterAttribute.ConverterType == typeof(DateConverter))
            schema.Format = DateFormat;
    }
}
