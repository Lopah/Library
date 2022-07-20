using JetBrains.Annotations;

namespace OpenApi.Attributes;

[PublicAPI]
public class SwaggerIfModifiedSinceParameter : SwaggerApiParameterAttribute
{
    public SwaggerIfModifiedSinceParameter(Type apiResources)
        : base("Time of last change, RFC7332 section 3.3", apiResources, "ModifiedSince")
    {
    }
}