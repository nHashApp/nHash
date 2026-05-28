using System.CommandLine;
using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Encodes;
using nHash.Console.CommandLines.Hashes;
using nHash.Console.CommandLines.Passwords;
using nHash.Console.CommandLines.Texts;
using nHash.Console.CommandLines.Uuids;
using nHash.Console.CommandLines.Ids;
using nHash.Console.CommandLines.Cryptos;
using nHash.Console.CommandLines.Converts;
using nHash.Console.CommandLines.Arts;
using nHash.Console.CommandLines.Network;
using nHash.Console.CommandLines.Date;
using nHash.Console.CommandLines.File;
using nHash.Console.CommandLines.Dev;
using nHash.Console.CommandLines.Sys;
using nHash.Console.CommandLines.Maths;
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
            Get<IIdCommand>(provider),
            Get<IConvertCommand>(provider),
            Get<ICryptoCommand>(provider),
            Get<ITextCommand>(provider),
            Get<IArtCommand>(provider),
            Get<INetworkCommand>(provider),
            Get<IDateCommand>(provider),
            Get<IFileCommand>(provider),
            Get<IDevCommand>(provider),
            Get<ISysCommand>(provider),
            Get<IMathCommand>(provider),
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