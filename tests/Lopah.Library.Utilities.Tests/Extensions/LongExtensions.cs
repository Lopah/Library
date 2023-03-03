using FluentAssertions;
using Utilities.Extensions;
using Xunit;

namespace Lopah.Library.Utilities.Tests.Extensions;

public class LongExtensions
{
    [Fact]
    public void LongExtensions_3LongNumber_ReturnsArrayOf3Ints()
    {
        var threeLongNumber = 123L;

        var expectedOutput = new[]
        {
            1, 2, 3
        };

        var result = threeLongNumber.ToIntArray();

        result.Should().BeEquivalentTo(expectedOutput);
    }
}