using System;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenApi.Extensions;
using OpenApi.Filters;
using OpenApi.Options;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenApi.Configuration;

[PublicAPI]
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly OpenApiOptions _openApiOptions;
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptions<OpenApiOptions> options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _openApiOptions = options.Value;
    }

    /// <summary>
    ///     Invoked to configure a  <paramref name="options"/> instance.
    /// </summary>
    /// <param name="options">The options instance to configure.</param>
    public void Configure(SwaggerGenOptions options)
    {
        if (_openApiOptions.Security.ApiKey is null)
        {
            options.AddOAuthSecurityDefinition(_openApiOptions.Security);
        }
        else
        {
            options.AddApiKeySecurityDefinition(_openApiOptions.Security.ApiKey);
        }

        AddXmlDocs(options);
        AddServers(options);

        options.EnableAnnotations();
        options.OrderActionsBy(desc => desc.GetValueForSorting());
        options.OperationFilter<AddResponseHeadersFilter>();
        options.SchemaFilter<PatternValidationAttributesFilter>();
        options.SchemaFilter<DataSchemaFilter>();
    }

    private void AddServers(SwaggerGenOptions options)
    {
        if (_openApiOptions.Servers == null && _openApiOptions.Servers?.Any() != true)
        {
            return;
        }

        foreach (var server in _openApiOptions.Servers)
        {
            options.AddServer(server);
        }
    }

    private void AddXmlDocs(SwaggerGenOptions options)
    {
        var xmlPath = Assembly.GetEntryAssembly()?.XmlCommentsFilePath();

        if (xmlPath == null || !Directory.Exists(xmlPath))
        {
            return;
        }

        foreach (var fileInfo in new DirectoryInfo(xmlPath).EnumerateFiles("*.xml"))
        {
            options.IncludeXmlComments(fileInfo.FullName, true);
        }
    }

    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = _openApiOptions.OpenApiInfo.Title,
            Version = description.ApiVersion.ToString(),
            Description = _openApiOptions.OpenApiInfo.Description,
            Contact = new()
            {
                Name = _openApiOptions.OpenApiInfo.Contact.Name,
                Email = _openApiOptions.OpenApiInfo.Contact.Email,
                Url = _openApiOptions.OpenApiInfo.Contact.Url
            },
            License = new()
            {
                Url = _openApiOptions.OpenApiInfo.Licence.Url,
                Name = _openApiOptions.OpenApiInfo.Licence.Name
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}