using System.Globalization;
using System.Text;

namespace Utilities.Extensions;

public static class ByteArrayExtensions
{
    public static string? ToHexString(this byte[]? bytes, bool upperCase = false)
    {
        if (bytes is null) return null;

        var format = upperCase ? "X2" : "x2";

        var builder = new StringBuilder(bytes.Length * 2);

        foreach (var b in bytes) builder.Append(b.ToString(format, CultureInfo.InvariantCulture));

        return builder.ToString();
    }
}
