using System;
using System.Linq;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using OpenApi.Configuration;
using OpenApi.Filters;
using OpenApi.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Extensions;

public static class OpenApiExtensions
{
    public static void AddVersionedOpenApi(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<SwaggerGenOptions>? setupAction = null)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        services.AddOptions<OpenApiOptions>()
            .Bind(configuration.GetSection(nameof(OpenApiOptions))).ValidateDataAnnotations();

        var openApiConfig = configuration.GetSection(nameof(OpenApiOptions)).Get<OpenApiOptions>();

        services.AddApiVersioning(
            options =>
            {
                if (openApiConfig?.DefaultApiVersion != null)
                {
                    options.DefaultApiVersion = new(
                        openApiConfig.DefaultApiVersion.Major!.Value,
                        openApiConfig.DefaultApiVersion.Minor!.Value);
                }

                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    options.SubstituteApiVersionInUrl = true;
                });

        if (openApiConfig?.DisableSwagger == true)
        {
            return;
        }

        services.AddEndpointsApiExplorer();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.AddSwaggerGen(
            options =>
            {
                options.OperationFilter<SwaggerFileOperationFilter>();
                setupAction?.Invoke(options);
            });
    }

    public static void UseVersionedOpenApi(
        this IApplicationBuilder app,
        IApiVersionDescriptionProvider provider,
        IOptions<OpenApiOptions> options)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (provider is null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var optionValue = options.Value;

        if (optionValue.DisableSwagger)
        {
            return;
        }

        app.UseSwagger(s =>
        {
            s.RouteTemplate = optionValue.RouteTemplate;
            s.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
        });

        app.UseSwaggerUI(
            opts =>
            {
                opts.RoutePrefix = optionValue.RoutePrefix;
                if (optionValue.Security.AuthorizationCode != null)
                {
                    opts.OAuthUsePkce();
                }

                var routeDepth = optionValue.RoutePrefix.Split('/', StringSplitOptions.RemoveEmptyEntries).Length;
                var relativePath = string.Concat(Enumerable.Repeat("../", routeDepth));
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var path = optionValue.RouteTemplate.Replace(
                        "{documentName}",
                        description.GroupName,
                        StringComparison.InvariantCultureIgnoreCase);

                    opts.SwaggerEndpoint($"{relativePath}{path.TrimStart('/')}", description.GroupName.ToUpperInvariant());
                }
            });
    }
}
