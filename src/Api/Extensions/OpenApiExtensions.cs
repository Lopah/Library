using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
                // TODO:
                // options.ErrorResponses = 
            });

        services.AddVersionedApiExplorer(
            options =>
            {
                // Adds IApiVersionDescriptionProvider service
                // "'v'major[.minor][-status]
                options.GroupNameFormat = "'v'VVV";

                options.SubstituteApiVersionInUrl = true;
            });

        if (openApiConfig?.DisableSwagger == true)
        {
            return;
        }

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
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

        app.UseSwagger(
            s =>
                s.RouteTemplate = optionValue.RouteTemplate);

        app.UseSwaggerUI(
            opts =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    opts.RoutePrefix = optionValue.RoutePrefix;
                    var path = optionValue.RouteTemplate.Replace(
                        "{documentName}",
                        description.GroupName,
                        StringComparison.InvariantCultureIgnoreCase);

                    opts.SwaggerEndpoint($"/{path}", description.GroupName.ToUpperInvariant());
                }
            });
    }
}