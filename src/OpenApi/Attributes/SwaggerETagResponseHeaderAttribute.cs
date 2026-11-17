using System.Net;
using JetBrains.Annotations;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;

namespace OpenApi.Attributes;

[PublicAPI]
public class SwaggerETagResponseHeaderAttribute : SwaggerResponseHeaderAttribute
{
    public SwaggerETagResponseHeaderAttribute()
        : base((int)HttpStatusCode.OK, "ETag", JsonSchemaType.String, "An identifier for a specific version of a resource.")
    {
    }
}
