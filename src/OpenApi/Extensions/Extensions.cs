using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using OpenApi.Options;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenApi.Extensions;

[PublicAPI]
internal static class Extensions
{
    internal static string XmlCommentsFilePath(this Assembly assembly)
    {
        var basePath = PlatformServices.Default.Application.ApplicationBasePath;

        return basePath;
    }

    internal static void AddOAuthSecurityDefinition(this SwaggerGenOptions options, SecurityOptions securityOptions)
    {
        options.AddSecurityDefinition("oauth2", new()
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = CreateOpenApiOAuthFlows(securityOptions)
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }

    internal static void AddApiKeySecurityDefinition(this SwaggerGenOptions options, ApiKeyOptions apiKeyOptions)
    {
        const string pskScheme = "psk";

        options.AddSecurityDefinition(pskScheme, new()
        {
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Name = apiKeyOptions.HeaderName,
            Description = apiKeyOptions.Description
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>(true, pskScheme);
    }

    private static OpenApiOAuthFlows CreateOpenApiOAuthFlows(SecurityOptions securityOptions)
    {
        var flows = new OpenApiOAuthFlows();

        if (securityOptions.ClientCredentials?.Scopes != null)
        {
            flows.ClientCredentials = new()
            {
                TokenUrl = securityOptions.ClientCredentials.TokenUrl,
                RefreshUrl = securityOptions.ClientCredentials.RefreshUrl,
                Scopes = securityOptions.ClientCredentials.Scopes.ToDictionary(key => key.Name, val => val.Description)
            };
        }

        if (securityOptions.Implicit?.Scopes != null)
        {
            flows.Implicit = new()
            {
                AuthorizationUrl = securityOptions.Implicit.AuthorizationUrl,
                Scopes = securityOptions.Implicit.Scopes.ToDictionary(key => key.Name, val => val.Description)
            };
        }

        if (securityOptions.AuthorizationCode?.Scopes != null)
        {
            flows.AuthorizationCode = new()
            {
                AuthorizationUrl = securityOptions.AuthorizationCode.AuthorizationUrl,
                TokenUrl = securityOptions.AuthorizationCode.TokenUrl,
                Scopes = securityOptions.AuthorizationCode.Scopes.ToDictionary(key => key.Name, val => val.Description)
            };
        }

        return flows;
    }
}