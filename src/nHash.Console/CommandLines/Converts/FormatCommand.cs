using System.CommandLine;
using nHash.Application.Converts;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Converts;

public class FormatCommand(IFormatConverterService formatConverterService, IOutputProvider outputProvider) : IFormatCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _inputArgument = new("input") { Description = "The input string content" };
    private readonly Option<string> _fromOption = new("--from", "-f") { Description = "Source format (json, yaml, xml)", Required = true };
    private readonly Option<string> _toOption = new("--to", "-t") { Description = "Target format (json, yaml, xml)", Required = true };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("format", "Convert direct structured data formats (json, yaml, xml)", GetExamples());
        command.Arguments.Add(_inputArgument);
        command.Options.Add(_fromOption);
        command.Options.Add(_toOption);

        command.SetAction(parseResult =>
        {
            var input = parseResult.GetValue(_inputArgument);
            var fromFormat = parseResult.GetValue(_fromOption);
            var toFormat = parseResult.GetValue(_toOption);
            
            var result = formatConverterService.Convert(input ?? string.Empty, fromFormat ?? string.Empty, toFormat ?? string.Empty);
            outputProvider.Append(result);
        });

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Convert JSON to YAML format", "nhash convert format \"{\\\"name\\\": \\\"nHash\\\"}\" -f json -t yaml"),
            new("Convert XML to JSON format", "nhash convert format \"<root><version>2.0</version></root>\" -f xml -t json")
        ];
}
