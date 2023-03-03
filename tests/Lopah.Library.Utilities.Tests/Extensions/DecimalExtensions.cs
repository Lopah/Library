using FluentAssertions;
using Utilities.Extensions;
using Xunit;

namespace Lopah.Library.Utilities.Tests.Extensions;

public class DecimalExtensions
{
    [Fact]
    public void EqualsWithPrecision_WithNumberWith6DecimalNumbersAnd3Precision_ReturnsTrue()
    {
        var num = 12.334123m;

        var other = 12.334m;


        var output = num.EqualsWithPrecision(other, 3);

        output.Should().BeTrue();
    }

    [Fact]
    public void EqualsWithPrecision_WithNumberWith6DecimalNumberAnd4Precision_ReturnsFalse()
    {
        var num = 12.334123m;

        var other = 12.334m;


        var output = num.EqualsWithPrecision(other, 4);

        output.Should().BeFalse();
    }
}