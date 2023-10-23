using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Utilities.Converters;
using Xunit;

namespace Lopah.Library.Utilities.Tests.Converters;

public class DateConverterTests
{
    [Fact]
    public void DateConverter_GivenDateInYYYYMMDDWithTimeZoneInfo_ConvertsItCorrectly()
    {
        const string json = """
            {
             "date": "1998-10-26T16:11:10.531Z"
            } 
        """;

        using var reader = new StringReader(json);

        var byteArray = Encoding.UTF8.GetBytes(json);
        var readonlySpan = new ReadOnlySpan<byte>(byteArray);

        var jsonReader = new Utf8JsonReader(
            readonlySpan,
            new JsonReaderOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Disallow,
                MaxDepth = 0
            });

        var output = JsonSerializer.Deserialize<Data>(
            ref jsonReader,
            new JsonSerializerOptions
            {
                Converters = { new DateTimeConverter() }
            });

        output.Should().NotBeNull();

        output!.Date.HasValue.Should().BeTrue();

        var date = output.Date!.Value;

        date.Year.Should().Be(1998);
        date.Month.Should().Be(10);
        date.Day.Should().Be(26);
        date.Hour.Should().Be(16);
        date.Minute.Should().Be(11);
        date.Second.Should().Be(10);
    }


    [Fact]
    public void DateConverter_GivenDateInYYYYDDMM_FailsToConvert()
    {
        {
            const string json = """
            {
             "date": "1998-26-10"
            } 
        """;


            var output = new Func<Data?>(
                () =>
                {
                    using var reader = new StringReader(json);

                    var byteArray = Encoding.UTF8.GetBytes(json);
                    var readonlySpan = new ReadOnlySpan<byte>(byteArray);


                    var jsonReader = new Utf8JsonReader(
                        readonlySpan,
                        new JsonReaderOptions
                        {
                            AllowTrailingCommas = true,
                            CommentHandling = JsonCommentHandling.Disallow,
                            MaxDepth = 0
                        });

                    return JsonSerializer.Deserialize<Data>(
                        ref jsonReader,
                        new JsonSerializerOptions
                        {
                            AllowTrailingCommas = true,
                            ReadCommentHandling = JsonCommentHandling.Disallow,
                            MaxDepth = 0,
                            Converters = { new DateOnlyConverter() }
                        });
                });

            output.Should().ThrowExactly<JsonException>().WithMessage("**LineNumber: 1**");
        }
    }

    [Fact]
    public void DateConverter_GivenDateInIsoFormat_ReturnsCorrectly()
    {
        const string json = """
            {
             "date": "2009-06-11T16:11:10.5312500Z"
            } 
        """;

        using var reader = new StringReader(json);

        var byteArray = Encoding.UTF8.GetBytes(json);
        var readonlySpan = new ReadOnlySpan<byte>(byteArray);

        var jsonReader = new Utf8JsonReader(
            readonlySpan,
            new JsonReaderOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Disallow,
                MaxDepth = 0
            });

        var output = JsonSerializer.Deserialize<Data>(
            ref jsonReader,
            new JsonSerializerOptions
            {
                Converters = { new DateTimeConverter() }
            });

        output.Should().NotBeNull();

        output!.Date.HasValue.Should().BeTrue();

        var date = output.Date!.Value;

        date.Year.Should().Be(2009);
        date.Month.Should().Be(6);
        date.Day.Should().Be(11);
        date.Hour.Should().Be(16);
        date.Minute.Should().Be(11);
        date.Second.Should().Be(10);
        date.Millisecond.Should().Be(531);
    }

    [Fact]
    public void DateConverter_GivenDateInLocalFormatWithTimeZoneInfo_ReturnsCorrectly()
    {
        const string json = """
            {
             "date": "2009-06-11T16:11:10.5312500+02:00"
            } 
        """;

        using var reader = new StringReader(json);

        var byteArray = Encoding.UTF8.GetBytes(json);
        var readonlySpan = new ReadOnlySpan<byte>(byteArray);

        var jsonReader = new Utf8JsonReader(
            readonlySpan,
            new JsonReaderOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Disallow,
                MaxDepth = 0
            });

        var output = JsonSerializer.Deserialize<Data>(
            ref jsonReader,
            new JsonSerializerOptions
            {
                Converters = { new DateTimeConverter() }
            });

        output.Should().NotBeNull();

        output!.Date.HasValue.Should().BeTrue();

        var date = output.Date!.Value;

        date.Year.Should().Be(2009);
        date.Month.Should().Be(6);
        date.Day.Should().Be(11);
        date.Hour.Should().Be(14);
        date.Minute.Should().Be(11);
        date.Second.Should().Be(10);
        date.Millisecond.Should().Be(531);
    }

    private class Data
    {
        [JsonPropertyName("date")]
        public DateTimeOffset? Date { get; set; }
    }
}