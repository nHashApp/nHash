namespace nHash.App;

public static class Startup
{
    public static void RegisterServices(IServiceCollection services)
    {
        Domain.ConfigureServices.Register(services);
        Application.ConfigureServices.Register(services);
        Infrastructure.ConfigureServices.Register(services);
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
            _ = Console.KeyAvailable;
        }
        catch (InvalidOperationException)
        {
            pipedText = await Console.In.ReadToEndAsync();
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