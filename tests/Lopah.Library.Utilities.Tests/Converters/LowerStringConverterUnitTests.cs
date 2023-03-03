using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Utilities.Converters;
using Xunit;

namespace Lopah.Library.Utilities.Tests.Converters;

public class LowerStringConverterTests
{
    private class Data
    {
        [JsonConverter(typeof(LowerStringConverter))]
        public string Something { get; set; } = string.Empty;

    }
    [Fact]
    public void LowerStringConverter_GivenUpperCaseString_LowersIt()
    {
        var json = """
            {
                "Something": "weirdLoL"
            }
        """;

        var data = JsonSerializer.Deserialize<Data>(json);


        data.Should().NotBeNull();

        data!.Something.Should().BeLowerCased();
    }
}