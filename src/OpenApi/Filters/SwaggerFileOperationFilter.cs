using System;
using System.Globalization;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenApi.Filters;

[PublicAPI]
public class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation is null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var binaryResponses = context.ApiDescription.SupportedResponseTypes
            .Where(x => x.Type == typeof(Stream) || x.Type == typeof(byte[]));

        foreach (var binaryResponse in binaryResponses)
        {
            var statusCode = binaryResponse.StatusCode.ToString(CultureInfo.InvariantCulture);
            if (operation.Responses == null ||
                !operation.Responses.TryGetValue(statusCode, out var apiResponse) ||
                apiResponse is not OpenApiResponse response)
            {
                continue;
            }

            response.Content ??= new System.Collections.Generic.Dictionary<string, OpenApiMediaType>();
            response.Content.Clear();
            response.Content["application/octet-stream"] = new()
            {
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Format = "binary"
                }
            };
        }
    }
}
