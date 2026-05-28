using System.CommandLine;
using nHash.Application.Converts;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Converts;

public class BaseNCommand(IBaseNService baseNService, IOutputProvider outputProvider) : IBaseNCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _numberArgument = new("number") { Description = "The number to convert" };
    private readonly Option<int> _fromBaseOption = new("--from", "-f") { Description = "Source base (2-36)", Required = true };
    private readonly Option<int> _toBaseOption = new("--to", "-t") { Description = "Target base (2-36)", Required = true };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base-n", "Convert numbers between arbitrary bases (2-36)", GetExamples());
        command.Arguments.Add(_numberArgument);
        command.Options.Add(_fromBaseOption);
        command.Options.Add(_toBaseOption);

        command.SetAction(parseResult =>
        {
            var number = parseResult.GetValue(_numberArgument);
            var fromBase = parseResult.GetValue(_fromBaseOption);
            var toBase = parseResult.GetValue(_toBaseOption);
            
            var result = baseNService.Convert(number ?? string.Empty, fromBase, toBase);
            outputProvider.Append(result);
        });

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Convert a binary number to hexadecimal", "nhash convert base-n 101010 -f 2 -t 16"),
            new("Convert a decimal number to base-36", "nhash convert base-n 123456789 -f 10 -t 36")
        ];
}
