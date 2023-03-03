using FluentAssertions;
using Utilities.Extensions;
using Xunit;

namespace Lopah.Library.Utilities.Tests.Extensions;

public class IntExtensions
{
    [Fact]
    public void IntExtensions_3LongNumber_Returns3()
    {
        var threeLongNumber = 123;

        var result = threeLongNumber.GetNumberOfDigits();

        result.Should().Be(3);
    }


    [Fact]
    public void IntExtensions_3LongNumber_ReturnsArrayOf3Ints()
    {
        var threeLongNumber = 123;

        var expectedOutput = new[]
        {
            1, 2, 3
        };

        var result = threeLongNumber.ToIntArray();

        result.Should().BeEquivalentTo(expectedOutput);
    }
}