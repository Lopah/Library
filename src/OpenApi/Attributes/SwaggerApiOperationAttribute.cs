using System.Reflection;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.Annotations;

namespace OpenApi.Attributes;

[PublicAPI]
public class SwaggerApiOperationAttribute : SwaggerOperationAttribute
{
    public SwaggerApiOperationAttribute(string tag)
    {
        Tag = tag;
        Tags = new[] { tag };
    }

    public SwaggerApiOperationAttribute(string defaultTag, string otherTag)
    {
        Tag = defaultTag;
        Tags = new[] { defaultTag, otherTag };
    }

    public SwaggerApiOperationAttribute(string defaultTag, string[] otherTags)
    {
        Tag = defaultTag;

        Tags = new List<string>(otherTags) { defaultTag }.ToArray();
    }

    public SwaggerApiOperationAttribute(
        string operationId,
        string defaultTag,
        Type resourceType,
        string? defaultSummary = null,
        string? defaultDescription = null)
    {
        if (resourceType is null)
        {
            throw new ArgumentNullException(nameof(resourceType));
        }

        var tag = GetTag(operationId, resourceType, defaultTag);
        Tag = tag;
        Tags = new[] { tag };
        OperationId = operationId;
        Summary = GetSummary(operationId, resourceType, defaultSummary);
        Description = GetDescription(operationId, resourceType, defaultDescription);
    }

    public string? Tag { get; }

    private string? GetTag(string operationId, Type resourceType, string defaultTag)
    {
        var tag = defaultTag;

        var tagProperty = resourceType.GetProperty(
            $"{operationId}{nameof(Tag)}",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

        if (tagProperty != null && tagProperty.PropertyType == typeof(string))
        {
            tag = tagProperty.GetValue(null, null) as string;
        }

        return tag;
    }

    private string? GetSummary(string operationId, Type resourceType, string? defaultSummary)
    {
        var summary = defaultSummary;

        var summaryProperty = resourceType.GetProperty(
            $"{operationId}{nameof(Summary)}",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

        if (summaryProperty != null && summaryProperty.PropertyType == typeof(string))
        {
            summary = summaryProperty.GetValue(null, null) as string;
        }

        return summary;
    }

    private string? GetDescription(string operationId, Type resourceType, string? defaultDescription)
    {
        var description = defaultDescription;

        var descriptionProperty = resourceType.GetProperty(
            $"{operationId}{nameof(Description)}",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

        if (descriptionProperty != null && descriptionProperty.PropertyType == typeof(string))
        {
            description = descriptionProperty.GetValue(null, null) as string;
        }

        return description;
    }
}