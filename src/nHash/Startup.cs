namespace nHash;

public static class Startup
{
    public static async Task<int> StartAsync(IEnumerable<string> args)
    {
        var features = GetFeatureClasses();

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

    private static IEnumerable<IFeature> GetFeatureClasses()
    {
        var res = new List<IFeature>();
        try
        {
            var featureType = typeof(IFeature);
            var types = typeof(Program).Assembly.GetTypes()
                .Where(_ => _ is { IsInterface: false, IsAbstract: false } && featureType.IsAssignableFrom(_))
                .Select(_ => Activator.CreateInstance(_) as IFeature);
            res.AddRange(types!);
        }
        catch
        {
            //
        }

        return res;
    }
}