using nHash.Application.Providers;

namespace nHash.Application;

public static class ConfigureServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IJsonTools, JsonTools>();
        services.AddSingleton<IUUIDGenerator, UUIDGenerator>();
    }
}