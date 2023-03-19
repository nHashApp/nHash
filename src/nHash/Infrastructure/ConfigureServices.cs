using nHash.Infrastructure.Helper.Logging;

namespace nHash.Infrastructure;

public static class ConfigureServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddLoggingExt();
    }
}