using nHash.Domain.Models;

namespace nHash.Domain;

public static class ConfigureServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<OutputParameter>();
    }
}