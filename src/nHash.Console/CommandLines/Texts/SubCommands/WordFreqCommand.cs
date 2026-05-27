using System.CommandLine;
using nHash.Application.Texts;
using nHash.Application.Abstraction;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class WordFreqCommand(ITextToolsService textToolsService, IOutputProvider outputProvider) : IWordFreqCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text") { Description = "Text to analyze word frequency" };
    private readonly Option<int> _topOption = new("--top", "-n") { Description = "Top N words to show", DefaultValueFactory = _ => 10 };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("wordfreq", "Count frequency of words in text", GetExamples());
        command.Arguments.Add(_textArgument);
        command.Options.Add(_topOption);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument) ?? string.Empty;
            var top = parseResult.GetValue(_topOption);
            var result = textToolsService.CountWordFrequency(text, top);
            outputProvider.AppendLine(result);
        });
        command.Aliases.Add("wf");
        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Word frequency analysis", "nhash text wordfreq \"the quick brown fox jumps over the lazy dog\""),
            new("Word frequency top 3", "nhash text wordfreq \"the quick brown fox jumps over the lazy dog\" -n 3")
        ];
}
