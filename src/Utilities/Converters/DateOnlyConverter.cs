using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Utilities.Converters;

[PublicAPI]
public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private string DatePattern = "yyyy-MM-dd";
    /// <summary>Reads and converts the JSON to type <see cref="DateOnly"/>.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(
            reader.GetString() ?? string.Empty,
            DatePattern,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal);
    }

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStringValue(value.ToString(DatePattern, CultureInfo.InvariantCulture));
    }
}