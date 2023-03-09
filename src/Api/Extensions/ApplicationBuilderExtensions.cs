using System;
using System.Linq;
using Api.Options;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Api.Extensions;

[PublicAPI]
public static class ApplicationBuilderExtensions
{
    public static void UseCorsPolicy(
        this IApplicationBuilder app,
        CorsOptions corsOptions)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        var options = corsOptions;

        app.UseCors(
            builder =>
            {
                builder
                    .WithOrigins(options.Origins.ToArray())
                    .WithMethods(options.Methods.ToArray())
                    .WithExposedHeaders(options.ExposedHeaders.ToArray())
                    .AllowAnyHeader();
            });
    }
    
    public static void UseCorsPolicy(
        this IApplicationBuilder app,
        IOptions<CorsOptions> corsOptions)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        var options = corsOptions.Value;

        app.UseCors(
            builder =>
            {
                builder
                    .WithOrigins(options.Origins.ToArray())
                    .WithMethods(options.Methods.ToArray())
                    .WithExposedHeaders(options.ExposedHeaders.ToArray())
                    .AllowAnyHeader();
            });
    }
}