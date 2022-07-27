using System.Net;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OpenApi.Attributes;

[PublicAPI]
public class SwaggerETagResponseHeaderAttribute : SwaggerResponseHeaderAttribute
{
    public SwaggerETagResponseHeaderAttribute()
        : base((int)HttpStatusCode.OK, "ETag", "int32", "An identifier for a specific version of a resource.")
    {
    }
}