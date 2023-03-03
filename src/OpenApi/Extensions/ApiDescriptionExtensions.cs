using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using OpenApi.Attributes;

namespace OpenApi.Extensions;

[PublicAPI]
public static class ApiDescriptionExtensions
{
    internal static string GetValueForSorting(this ApiDescription apiDescription)
    {
        var operation = apiDescription.ActionDescriptor.EndpointMetadata
            .FirstOrDefault(x => x is SwaggerApiOperationAttribute);

        if (operation is not SwaggerApiOperationAttribute swaggerOperation)
        {
            return apiDescription.GroupName ?? string.Empty;
        }

        return swaggerOperation.Tag ?? string.Empty;
    }
}