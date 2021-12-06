using System.Globalization;

namespace Utilities.Extensions;

public static class DateTimeOffsetExtensions
{
    public static bool IsDateValid(this DateTimeOffset date)
    {
        return !date.Equals(default);
    }

    public static DateTimeOffset GetTheLocalStartOfTheDay(this DateTimeOffset date)
    {
        return date.LocalDateTime.Date;
    }

    public static DateTimeOffset TruncateMicroseconds(this DateTimeOffset date)
    {
        return new(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Offset);
    }

    public static DateTimeOffset TruncateMilliseconds(this DateTimeOffset date)
    {
        return new(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Offset);
    }

    public static DateTimeOffset RoundUpMilliseconds(this DateTimeOffset date)
    {
        var ticks = date.Ticks % TimeSpan.TicksPerMillisecond;

        if (ticks >= 5_000) return date.AddTicks(10_000 - ticks);

        return date.AddTicks(-ticks);
    }

    public static string ToFormattedLocalDate(this DateTimeOffset dateTimeValue, string format)
    {
        return dateTimeValue.LocalDateTime.ToString(format, CultureInfo.InvariantCulture);
    }

    public static IReadOnlyCollection<DateTimeOffset> GetLocalDatesOutOfRange(this DateTimeOffset sinceDate,
        DateTimeOffset untilDate)
    {
        var daysCount = untilDate.LocalDateTime.Date.Subtract(sinceDate.LocalDateTime.Date).Days;
        return Enumerable.Range(0, 1 + daysCount)
            .Select(offset => sinceDate.AddLocalDays(offset))
            .ToList();
    }

    public static DateTimeOffset AddWithRespectingDaylightSavings(this DateTimeOffset dateTime, TimeSpan span)
    {
        if (dateTime.Offset == TimeSpan.Zero) return dateTime + span;

        var shiftedUtcDateTime = dateTime.UtcDateTime + span;

        return new DateTimeOffset(shiftedUtcDateTime, TimeSpan.Zero)
            .ToLocalTime();
    }

    public static int GetLocalDays(this DateTimeOffset sinceDate, DateTimeOffset untilDate)
    {
        var untilDateIncluded = untilDate.AddLocalDays(1);
        return untilDateIncluded.LocalDateTime.Date.Subtract(sinceDate.LocalDateTime.Date).Days;
    }

    /// <summary>
    ///     Keeps the time of the day, changes offset
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="months"></param>
    /// <returns></returns>
    public static DateTimeOffset AddLocalMonths(this DateTimeOffset dateTime, int months)
    {
        var newLocal = dateTime.LocalDateTime.AddMonths(months);

        return new(newLocal, TimeZoneInfo.Local.GetUtcOffset(newLocal));
    }

    /// <summary>
    ///     Keeps the time of the day, changes offset
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    public static DateTimeOffset AddLocalDays(this DateTimeOffset dateTime, int days)
    {
        var newLocal = dateTime.LocalDateTime.AddDays(days);

        return new(newLocal, TimeZoneInfo.Local.GetUtcOffset(newLocal));
    }

    public static DateTimeOffset GetBeginningOfFirstDayOfMonth(this DateTimeOffset dateTime)
    {
        var timeToSubtract = dateTime.TimeOfDay + TimeSpan.FromDays(dateTime.Day - 1);

        return dateTime.AddWithRespectingDaylightSavings(-timeToSubtract);
    }
}
