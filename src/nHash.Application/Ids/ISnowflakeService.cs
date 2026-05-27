namespace nHash.Application.Ids;

public interface ISnowflakeService
{
    long Generate(int workerId);
}
