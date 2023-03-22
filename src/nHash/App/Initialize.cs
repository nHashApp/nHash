using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using nHash.Application.Encodes;
using nHash.Application.Hashes;
using nHash.Application.Passwords;
using nHash.Application.Texts;
using nHash.Application.Uuids;
using nHash.Domain.Models;
using Spectre.Console;

namespace nHash.App;

public static class Initialize
{
    private const string OutputOption = "output";

    public static async Task<int> InitializeCommandLine(IEnumerable<string> args, IServiceProvider provider)
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

        var outputFileName = new Option<string>(name: "--" + OutputOption, description: "File name for writing output");
        rootCommand.AddGlobalOption(outputFileName);

        var commandLineBuilder = new CommandLineBuilder(rootCommand)
            .AddMiddleware((context, next) => OutputMiddleware(provider, context, next))
            .UseDefaults() //.UseVersionOption(new []{"--help"});
            .UseHelp(ctx =>
            {
                ctx.HelpBuilder.CustomizeLayout(
                    _ =>
                        HelpBuilder.Default
                            .GetLayout()
                            .Skip(1) // Skip the default command description section.
                            .Prepend(
                                _ => AnsiConsole.Write(
                                    new FigletText("nHash")
                                        .Centered())
                            ));
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

    private static void SetOutputOptions(IServiceProvider provider, InvocationContext context)
    {
        var outputParameter = provider.GetService<OutputParameter>()!;
        outputParameter.Type = OutputType.Console;

        var outputOption = context.ParseResult.CommandResult.Children.Where(_ => _.GetType() == typeof(OptionResult))
            .FirstOrDefault(_ => ((OptionResult)_).Option.Name == OutputOption);
        if (outputOption is not null)
        {
            outputParameter.Type = OutputType.File;
            outputParameter.OutputTypeValue = outputOption.Tokens[0].ToString();
        }
    }

    private static async Task WriteOutput(IServiceProvider provider)
    {
        var outputProvider = provider.GetService<IOutputProvider>()!;
        await outputProvider.WriteOutput();
    }
}