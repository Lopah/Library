using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utilities.Converters;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    /// <summary>Reads and converts the JSON to type <typeparamref name="T" />.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!TimeSpan.TryParse(reader.GetString(), CultureInfo.InvariantCulture, out var result))
            throw new JsonException();

        return result;
    }

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        if (writer is null) throw new ArgumentNullException(nameof(writer));

        writer.WriteStringValue(value.ToString(@"hh\:mm", CultureInfo.InvariantCulture));
    }
}
