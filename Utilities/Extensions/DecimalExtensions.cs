namespace Utilities.Extensions;

public static class DecimalExtensions
{
    public static bool EqualsWithPrecision(this decimal value, decimal other, int precisionDigits = 2)
    {
        return decimal.Round(value, precisionDigits) == decimal.Round(other, precisionDigits);
    }
}