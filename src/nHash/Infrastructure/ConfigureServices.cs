namespace nHash.Infrastructure;

public static class ConfigureServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IFileProvider, FileProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IOutputProvider, OutputProvider>();
    }
}