using System.Net;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace OpenApi.Attributes;

[PublicAPI]
public class SwaggerVaryResponseHeaderAttribute : SwaggerResponseHeaderAttribute
{
    public SwaggerVaryResponseHeaderAttribute()
        : base((int)HttpStatusCode.OK, "Vary", "string",
            "Indicating which headers are used when selecting a representation of a resource")
    {
    }
}
