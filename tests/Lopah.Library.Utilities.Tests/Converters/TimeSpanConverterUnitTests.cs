using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Utilities.Converters;
using Xunit;

namespace Lopah.Library.Utilities.Tests.Converters;

public class TimeSpanConverterTests
{
    private class Data
    {
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Something { get; set; }
        
    }
    [Fact]
    public void LowerStringConverter_GivenUpperCaseString_LowersIt()
    {
        var json = """
            {
                "Something": "10:10:10"
            }
        """;

        var data = JsonSerializer.Deserialize<Data>(json);


        data.Should().NotBeNull();

        data!.Something.Hours.Should().Be(10);
        data.Something.Minutes.Should().Be(10);
        data.Something.Seconds.Should().Be(10);
    }
}