using JetBrains.Annotations;

namespace Utilities.Extensions;

[PublicAPI]
public static class LongExtensions
{
    public static int[] ToIntArray(this long n)
    {
        if (n == 0)
        {
            return new[] { 0 };
        }

        var digits = new List<int>();

        for (; n != 0; n /= 10)
        {
            digits.Add((int)(n % 10));
        }

        var arr = digits.ToArray();
        Array.Reverse(arr);

        return arr;
    }
}