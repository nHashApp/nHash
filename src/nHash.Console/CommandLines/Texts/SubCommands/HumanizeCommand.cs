using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Humanizers.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class HumanizeCommand : IHumanizeCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument;
    private readonly Argument<HumanizeType> _humanizeType;

    private readonly IHumanizeService _humanizeService;
    private readonly IOutputProvider _outputProvider;

    public HumanizeCommand(IHumanizeService humanizeService, IOutputProvider outputProvider)
    {
        _humanizeService = humanizeService;
        _outputProvider = outputProvider;
        _humanizeType = new Argument<HumanizeType>("type", "Humanize type");
        _textArgument = new Argument<string>("text", "Text for humanize");
    }

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("humanize",
            "Humanizer text (Pascal-case, Camel-case, Kebab, Underscore, lowercase, etc)", GetExamples());
        command.AddArgument(_humanizeType);
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _humanizeType);
        command.AddAlias("h");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples()
    {
        return new List<KeyValuePair<string, string>>()
        {
            new("Convert text to CamelCase", "nhash text humanize camel \"this is a text\""),
            new("Convert text to kebab-case", "nhash text humanize kebab \"This is a Text\""),
            new("Convert text to snake_case", "nhash t h underscore \"This is a Text\""),
            new("Convert a humanized text to a normal string", "nhash text humanize dehumanize \"ThisIsAText\""),
        };
    }

    private void CalculateText(string text, HumanizeType humanizeType)
    {
        var resultText = _humanizeService.CalculateText(text, humanizeType);
        _outputProvider.AppendLine(resultText);
    }
}