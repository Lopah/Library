using System.Globalization;
using System.Text;

namespace Utilities.Extensions;

public static class StringExtensions
{
    public static string RemoveWhiteSpace(this string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        var trimmedCharacters = text.Where(c => !char.IsWhiteSpace(c)).ToArray();

        return new(trimmedCharacters);
    }

    public static string RemoveAccents(this string text)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        return string.Concat(
                text.Normalize(NormalizationForm.FormD).Where(ch =>
                    CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark))
            .Normalize(NormalizationForm.FormC);
    }

    public static string ReplaceOtherThan(this string text, HashSet<char> allowedChars, char charToReplace)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (allowedChars is null)
        {
            throw new ArgumentNullException(nameof(allowedChars));
        }

        var chars = text.ToCharArray();

        for (var i = 0; i < chars.Length; ++i)
        {
            var currentChar = chars[i];

            if (!allowedChars.Contains(currentChar))
            {
                chars[i] = charToReplace;
            }
        }

        return new(chars);
    }

    public static string TakeLastNumberOfCharacters(this string text, int numberOfCharacters)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        return text[Math.Max(0, text.Length - numberOfCharacters)..];
    }

    public static IReadOnlyList<string> SplitToChunks(this string text, int chunkSize)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (chunkSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkSize),
                "Chunk size parameter has to be greater than zero.");
        }

        if (text.Length % chunkSize != 0)
        {
            throw new ArgumentException("String is not divisible by specified chunk size without a remainder.",
                nameof(chunkSize));
        }

        return Enumerable.Range(0, text.Length / chunkSize)
            .Select(x => text.Substring(x * chunkSize, chunkSize))
            .ToList();
    }

    public static string ReduceLength(this string text, int maxLength)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (maxLength < 3)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength));
        }

        if (text.Length <= maxLength)
        {
            return text;
        }

        return text.Substring(0, maxLength - 3) + "...";
    }
}