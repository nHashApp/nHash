namespace nHash.Infrastructure;

public interface IDateTimeProvider
{
    DateTime FromUnixTime(long seconds);
}