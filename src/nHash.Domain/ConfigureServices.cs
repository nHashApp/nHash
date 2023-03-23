using Microsoft.Extensions.DependencyInjection;
using nHash.Domain.Models;

namespace nHash.Domain;

public static class ConfigureServices
{
    public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<OutputParameter>();
        
        return services;
    }
}