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

        var swaggerOperation = operation as SwaggerApiOperationAttribute;

        if (swaggerOperation is null)
        {
            return apiDescription.GroupName ?? string.Empty;
        }

        return swaggerOperation.Tag ?? string.Empty;
    }
}