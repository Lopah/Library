using System.Net;
using JetBrains.Annotations;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;

namespace OpenApi.Attributes;

[PublicAPI]
public class SwaggerVaryResponseHeaderAttribute : SwaggerResponseHeaderAttribute
{
    public SwaggerVaryResponseHeaderAttribute()
        : base(
            (int)HttpStatusCode.OK,
            "Vary",
            JsonSchemaType.String,
            "Indicating which headers are used when selecting a representation of a resource")
    {
    }
}
