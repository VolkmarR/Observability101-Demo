namespace Observability101.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Truncates the seconds and milliseconds from the given <see cref="DateTime"/> object,
    /// returning a new instance with time set to the minute precision.
    /// </summary>
    /// <param name="timeStamp">The <see cref="DateTime"/> instance to truncate.</param>
    /// <returns>A new <see cref="DateTime"/> object truncated to the minute.</returns>
    public static DateTime TruncateTimeToMinute(this DateTime timeStamp) => new(timeStamp.Year, timeStamp.Month,
        timeStamp.Day, timeStamp.Hour, timeStamp.Minute, 0, timeStamp.Kind);
}