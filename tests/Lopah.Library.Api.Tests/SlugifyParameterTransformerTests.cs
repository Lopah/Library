using Api;
using FluentAssertions;
using Xunit;

namespace Lopah.Library.Api.Tests;

public class SlugifyParameterTransformerTests
{
    [Theory]
    [InlineData(null,null)]
    [InlineData("", null)]
    [InlineData("TypicalControllerName", "typical-controller-name")]
    public void SlugifyParameterTransformer_ShouldTransformMixedCaseString(object? value, string? expected)
    {
        var transformer = new SlugifyParameterTransformer();

        var result = transformer.TransformOutbound(value);

        result.Should().Be(expected);
    }
}