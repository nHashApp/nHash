namespace nHash.Infrastructure;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime FromUnixTime(long seconds)
        => DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime;
}