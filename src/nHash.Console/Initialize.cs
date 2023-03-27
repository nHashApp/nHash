using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
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
    private static readonly Option<string> OutputFileName;

    static Initialize()
    {
        OutputFileName = new Option<string>(name: "--output", description: "File name for writing output");
        OutputFileName.AddAlias("-o");
    }

    public static async Task<int> InitializeCommandLine(IEnumerable<string> args, IServiceProvider provider)
    {
        var features = new List<IFeature>()
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
            rootCommand.AddCommand(feature.Command);
        }

        rootCommand.AddGlobalOption(OutputFileName);

        var commandLineBuilder = new CommandLineBuilder(rootCommand)
            .AddMiddleware((context, next) => OutputMiddleware(provider, context, next))
            .UseDefaults()
            .UseExceptionHandler(UnhandledExceptionMiddleware)
            .UseCustomVersionHandler()
            .UseHelp(ctx =>
            {
                ctx.HelpBuilder.CustomizeLayout(
                    _ =>
                        HelpBuilder.Default
                            .GetLayout()
                            .Append(WriteExampleSection)
                );
            });
        
        var parser = commandLineBuilder.Build();
        return await parser.InvokeAsync(args.ToArray());
        //return await rootCommand.InvokeAsync(parameters);
    }

    private static T Get<T>(IServiceProvider provider)
        where T : IFeature
        => provider.GetService<T>()!;

    private static async Task OutputMiddleware(IServiceProvider provider, InvocationContext context,
        Func<InvocationContext, Task> next)
    {
        SetOutputOptions(provider, context);

        await next(context);

        await WriteOutput(provider);
    }

    private static void UnhandledExceptionMiddleware(Exception exception, InvocationContext context)
    {
        System.Console.WriteLine($"Internal exception has occurred, {exception.Message}");
    }

    private static void WriteExampleSection(HelpContext context)
    {
        if (context.Command is not BaseCommand command)
        {
            return;
        }

        if (command.Examples is null)
        {
            return;
        }

        context.Output.WriteLine("Examples:");
        context.HelpBuilder.WriteColumns(
            command.Examples.Select(_ => new TwoColumnHelpRow(_.Key, _.Value)).ToList().AsReadOnly(), context);
    }

    private static void SetOutputOptions(IServiceProvider provider, InvocationContext context)
    {
        var outputParameter = provider.GetService<OutputParameter>()!;
        outputParameter.Type = OutputType.Console;

        var outputOption = context.ParseResult.CommandResult.Children
            .FirstOrDefault(_ => _.Symbol == OutputFileName);
        if (outputOption is null)
        {
            return;
        }

        outputParameter.Type = OutputType.File;
        outputParameter.OutputTypeValue = outputOption.Tokens[0].ToString();
    }

    private static async Task WriteOutput(IServiceProvider provider)
    {
        var outputProvider = provider.GetService<IOutputProvider>()!;
        await outputProvider.WriteOutput();
    }
}