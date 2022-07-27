using System;
using System.Net.Mime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

[PublicAPI]
public static class MvcExtensions
{
    public static void AddApiFilters(this MvcOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        options.ReturnHttpNotAcceptable = true;
        options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
        options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
    }
}