using nHash.Application;
using nHash.Application.Encodes;
using nHash.Application.Hashes;
using nHash.Application.Passwords;
using nHash.Application.Texts;
using nHash.Application.Uuids;

namespace nHash;

public static class Startup
{
    public static void RegisterServices(IServiceCollection services)
    {
        Application.ConfigureServices.Register(services);
        Infrastructure.ConfigureServices.Register(services);
    }

    public static async Task<int> StartAsync(IEnumerable<string> args, IServiceProvider provider)
    {
        var features = new List<IFeature>()
        {
            Get<IUuidFeature>(provider),
            Get<IEncodeFeature>(provider),
            Get<IHashFeature>(provider),
            Get<ITextFeature>(provider),
            Get<IPasswordFeature>(provider),
        };

        var rootCommand = new RootCommand("Hash and Text utilities in command-line mode");
        foreach (var feature in features)
        {
            rootCommand.AddCommand(feature.Command);
        }

        var parameters = await GetParameters(args);

        return await rootCommand.InvokeAsync(parameters);
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

    private static T Get<T>(IServiceProvider provider)
        where T : IFeature
        => provider.GetService<T>()!;
}