using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Humanizers.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class HumanizeCommand(IHumanizeService humanizeService, IOutputProvider outputProvider) : IHumanizeCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument = new("text") { Description = "Text for humanize" };
    private readonly Argument<HumanizeType> _humanizeType = new("type") { Description = "Humanize type" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("humanize",
            "Humanizer text (Pascal-case, Camel-case, Kebab, Underscore, lowercase, etc)", GetExamples());
        command.Arguments.Add(_humanizeType);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var humanizeType = parseResult.GetValue(_humanizeType);
            CalculateText(text ?? string.Empty, humanizeType);
        });
        command.Aliases.Add("h");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Convert text to CamelCase", "nhash text humanize camel \"this is a text\""),
            new("Convert text to kebab-case", "nhash text humanize kebab \"This is a Text\""),
            new("Convert text to snake_case", "nhash t h underscore \"This is a Text\""),
            new("Convert a humanized text to a normal string", "nhash text humanize dehumanize \"ThisIsAText\""),
        ];

    private void CalculateText(string text, HumanizeType humanizeType)
    {
        var resultText = humanizeService.CalculateText(text, humanizeType);
        outputProvider.AppendLine(resultText);
    }
}