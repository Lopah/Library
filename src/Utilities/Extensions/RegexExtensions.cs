using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Utilities.Extensions;

[PublicAPI]
public static class RegexExtensions
{
    public static bool IsValid(this Regex regex, object? value)
    {
        if (regex is null)
        {
            throw new ArgumentNullException(nameof(regex));
        }

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (value is not string stringValue)
        {
            return false;
        }

        return regex.IsMatch(stringValue);
    }

    public static Regex GetCompiledRegex(this string pattern)
    {
        return new(pattern, RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
}