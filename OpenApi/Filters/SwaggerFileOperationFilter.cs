using System.Globalization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenApi.Filters;

public class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation is null) throw new ArgumentNullException(nameof(operation));

        if (context is null) throw new ArgumentNullException(nameof(context));

        var binaryResponses = context.ApiDescription.SupportedResponseTypes
            .Where(x => x.Type == typeof(Stream) || x.Type == typeof(byte[]));

        foreach (var binaryResponse in binaryResponses)
        {
            var statusCode = binaryResponse.StatusCode.ToString(CultureInfo.InvariantCulture);
            var response = operation.Responses[statusCode];
            response.Content.Clear();
            response.Content["application/octet-stream"] = new()
            {
                Schema = new()
                {
                    Type = "string",
                    Format = "binary"
                }
            };
        }
    }
}
