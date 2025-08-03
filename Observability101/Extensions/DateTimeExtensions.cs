namespace Observability101.Extensions;

public static class DateTimeExtensions
{
    public static DateTime TruncateTimeToMinute(this DateTime timeStamp) => new(timeStamp.Year, timeStamp.Month,
        timeStamp.Day, timeStamp.Hour, timeStamp.Minute, 0, timeStamp.Kind);
}