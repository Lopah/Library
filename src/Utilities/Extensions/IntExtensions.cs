using System;
using JetBrains.Annotations;

namespace Utilities.Extensions;

[PublicAPI]
public static class IntExtensions
{
    public static int GetNumberOfDigits(this int n)
    {
        if (n < 0)
        {
            n = n == int.MinValue ? int.MaxValue : -n;
        }

        if (n == 0)
        {
            n = 1;
        }

        return (int)Math.Floor(Math.Log10(n)) + 1;
    }

    public static int[] ToIntArray(this int n)
    {
        var result = new int[GetNumberOfDigits(n)];

        for (var i = result.Length - 1; i >= 0; i--)
        {
            result[i] = n % 10;
            n /= 10;
        }

        return result;
    }
}