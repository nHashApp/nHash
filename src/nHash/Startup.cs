
namespace nHash;

public static class Startup
{
    public static async Task<int> StartAsync(IEnumerable<string> args)
    {
        var features = new List<IFeature>()
        {
            new GuidFeature(),
            new Md5Feature(),
            new Sha1Feature(),
            new Sha256Feature(),
            new Sha384Feature(),
            new Sha512Feature(),
            new Base64Feature(),
        };

        var rootCommand = new RootCommand("Hash utilities in command-line mode");
        foreach (var command in features)
        {
            rootCommand.AddCommand(command.Command);
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
}