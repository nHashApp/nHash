using Microsoft.Extensions.DependencyInjection;

namespace nHash.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileProvider, FileProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IOutputProvider, OutputProvider>();
        
        return services;
    }
}