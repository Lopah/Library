using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utilities.Converters;

public class DateConverter : JsonConverter<DateTimeOffset?>
{
    private const string Pattern = "yyyy-MM-dd";

    private static readonly string[] _iso8601Patterns =
        { "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK", "yyyy'-'MM'-'dd'T'HH':'mm':'ssK", Pattern };

    /// <summary>Reads and converts the JSON to type <typeparamref name="T" />.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var parsed = DateTimeOffset.TryParseExact(
            reader.GetString(),
            _iso8601Patterns,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeLocal,
            out var dt);

        return parsed ? dt.LocalDateTime.Date : null;
    }

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (writer is null) throw new ArgumentNullException(nameof(writer));

        if (value.HasValue)
        {
            writer.WriteStringValue(
                value.Value.LocalDateTime.ToString(Pattern, CultureInfo.InvariantCulture));

            return;
        }

        writer.WriteNullValue();
    }
}
