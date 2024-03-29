﻿using System;
using System.Reflection;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Utilities.Converters;

namespace OpenApi.Filters;

[PublicAPI]
public class DataSchemaFilter : ISchemaFilter
{
    private const string DateFormat = "date";

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

        var converterAttribute = context.MemberInfo?.GetCustomAttribute<JsonConverterAttribute>();

        if (converterAttribute != null && converterAttribute.ConverterType == typeof(DateTimeConverter))
        {
            schema.Format = DateFormat;
        }
    }
}