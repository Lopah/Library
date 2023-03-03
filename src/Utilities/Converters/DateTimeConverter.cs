using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Utilities.Converters;

[PublicAPI]
public class DateTimeConverter : JsonConverter<DateTimeOffset?>
{
    private static readonly string[] _iso8601Patterns =
        { "yyyy-MM-ddTHH:mm:ss.fffffffK", "yyyy-MM-ddTHH:mm:ssK", "yyyy-MM-ddTHH:mm:ss.fffK" };

    /// <summary>Reads and converts the JSON to type <see cref="DateTimeOffset" />.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="type">Type to convert</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type? type, JsonSerializerOptions? options)
    {
        var parsed = DateTimeOffset.TryParseExact(
            reader.GetString(),
            _iso8601Patterns,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal,
            out var dt);

        return parsed ? dt.ToUniversalTime() : null;
    }

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (value.HasValue)
        {
            writer.WriteStringValue(
                value.Value.LocalDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.InvariantCulture));

            return;
        }

        writer.WriteNullValue();
    }
}