using System.CommandLine;
using nHash.Application.Texts;
using nHash.Application.Abstraction;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class CountCommand(ITextToolsService textToolsService, IOutputProvider outputProvider) : ICountCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text") { Description = "Text to search in" };
    private readonly Option<string> _patternOption = new("--pattern", "-p") { Description = "Pattern or substring to search for", Required = true };
    private readonly Option<bool> _regexOption = new("--regex", "-r") { Description = "Treat the pattern as a regular expression", DefaultValueFactory = _ => false };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("count", "Count occurrences of a pattern in text", GetExamples());
        command.Arguments.Add(_textArgument);
        command.Options.Add(_patternOption);
        command.Options.Add(_regexOption);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument) ?? string.Empty;
            var pattern = parseResult.GetValue(_patternOption) ?? string.Empty;
            var useRegex = parseResult.GetValue(_regexOption);
            var result = textToolsService.CountOccurrences(text, pattern, useRegex);
            outputProvider.AppendLine(result);
        });
        command.Aliases.Add("cnt");
        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Count substring occurrences", "nhash text count \"the quick brown fox\" --pattern \"the\""),
            new("Count regex pattern occurrences", "nhash text count \"123abc456def\" --pattern \"\\d+\" --regex")
        ];
}
