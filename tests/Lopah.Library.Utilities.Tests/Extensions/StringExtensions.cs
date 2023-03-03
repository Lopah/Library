using FluentAssertions;
using Utilities.Extensions;
using Xunit;

namespace Lopah.Library.Utilities.Tests.Extensions;

public class StringExtensions
{
    [Fact]
    public void ReduceLength_With5MaxLengthStringAndEntered5LengthString_ReturnsItBack()
    {
        var @string = "abcde";

        var result = @string.ReduceLength(5);

        result.Should().BeEquivalentTo(@string);
    }

    [Fact]
    public void ReduceLength_With5MaxLengthStringAndEntered6LengthString_ShortensIt()
    {
        var @string = "abcdef";

        var expectedString = "ab...";

        var result = @string.ReduceLength(5);

        result.Should().BeEquivalentTo(expectedString);
    }
}