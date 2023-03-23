using nHash.Application;
using nHash.Domain;
using nHash.Infrastructure;

namespace nHash.Console;

public static class Startup
{
    public static void RegisterServices(IServiceCollection services)
    {
        services
            .RegisterDomainServices()
            .RegisterApplicationServices()
            .RegisterInfrastructureServices();
    }

    public static async Task<int> StartAsync(IEnumerable<string> args, IServiceProvider provider)
    {
        var parameters = await GetParameters(args);

        return await Initialize.InitializeCommandLine(parameters, provider);
    }

    private static async Task<string[]> GetParameters(IEnumerable<string> strings)
    {
        var pipedText = "";
        try
        {
            _ = System.Console.KeyAvailable;
        }
        catch (InvalidOperationException)
        {
            pipedText = await System.Console.In.ReadToEndAsync();
        }

        var list = new List<string>();
        list.AddRange(strings);
        if (!string.IsNullOrWhiteSpace(pipedText))
        {
            list.Add(pipedText);
        }

        return list.Where(_ => !string.IsNullOrWhiteSpace(_)).ToArray();
    }
}