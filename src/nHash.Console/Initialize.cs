using System.CommandLine;
using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Encodes;
using nHash.Console.CommandLines.Hashes;
using nHash.Console.CommandLines.Passwords;
using nHash.Console.CommandLines.Texts;
using nHash.Console.CommandLines.Uuids;
using nHash.Console.Helper;
using nHash.Domain.Models;

namespace nHash.Console;

public static class Initialize
{
    private static readonly Option<string> OutputFileName = InitializeOutput();

    public static async Task<int> InitializeCommandLine(IEnumerable<string> args, IServiceProvider provider)
    {
        var features = new List<IFeature>
        {
            Get<IUuidCommand>(provider),
            Get<IEncodeCommand>(provider),
            Get<IHashCommand>(provider),
            Get<ITextCommand>(provider),
            Get<IPasswordCommand>(provider),
        };

        var rootCommand = new RootCommand("Hash and Text utilities in command-line mode");
        foreach (var feature in features)
        {
            rootCommand.Subcommands.Add(feature.Command);
        }

        rootCommand.Options.Add(OutputFileName);
        rootCommand.UseCustomVersionHandler();

        var parseResult = rootCommand.Parse(args.ToArray());

        SetOutputOptions(provider, parseResult);

        int exitCode;
        try
        {
            exitCode = await parseResult.InvokeAsync();
        }
        catch (Exception exception)
        {
            System.Console.WriteLine("Internal exception has occurred");
            System.Console.WriteLine($"Detail: {exception.Message}");
            exitCode = 1;
        }

        await WriteOutput(provider);

        return exitCode;
    }

    private static T Get<T>(IServiceProvider provider)
        where T : IFeature
        => provider.GetService<T>()!;

    private static Option<string> InitializeOutput()
    {
        var outputOption = new Option<string>("--output", "-o")
        {
            Description = "File name for writing output",
            Recursive = true
        };
        return outputOption;
    }
    
    private static void SetOutputOptions(IServiceProvider provider, ParseResult parseResult)
    {
        var outputParameter = provider.GetService<OutputParameter>()!;
        outputParameter.Type = OutputType.Console;

        var outputValue = parseResult.GetValue(OutputFileName);
        if (string.IsNullOrWhiteSpace(outputValue))
        {
            return;
        }

        outputParameter.Type = OutputType.File;
        outputParameter.OutputTypeValue = outputValue;
    }

    private static async Task WriteOutput(IServiceProvider provider)
    {
        var outputProvider = provider.GetService<IOutputProvider>()!;
        await outputProvider.WriteOutput();
    }
}