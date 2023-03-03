using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Utilities.Converters;
using Xunit;

namespace Lopah.Library.Utilities.Tests.Converters;

public class TrimmingConverterTests
{
    private class Data
    {
        [JsonConverter(typeof(TrimmingConverter))]
        public string Something { get; set; } = string.Empty;

    }
    [Fact]
    public void LowerStringConverter_GivenUpperCaseString_LowersIt()
    {
        var json = """
            {
                "Something": "test!!!!    "
            }
        """;

        var data = JsonSerializer.Deserialize<Data>(json);


        data.Should().NotBeNull();


        var output = "test!!!!";

        data!.Something.Should().Be(output);
        data.Something.Length.Should().Be(output.Length);
    }
}