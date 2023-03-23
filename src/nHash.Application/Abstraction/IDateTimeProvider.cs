namespace nHash.Application.Abstraction;

public interface IDateTimeProvider
{
    DateTime FromUnixTime(long seconds);
}