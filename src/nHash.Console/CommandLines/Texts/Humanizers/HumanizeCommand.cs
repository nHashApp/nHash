using Humanizer;
using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Humanizers.Models;

namespace nHash.Console.CommandLines.Texts.Humanizers;

public class HumanizeCommand : IHumanizeCommand
{
    public Command Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument;
    private readonly Argument<HumanizeType> _humanizeType;

    private readonly IHumanizeService _humanizeService;

    public HumanizeCommand(IHumanizeService humanizeService)
    {
        _humanizeService = humanizeService;
        _humanizeType = new Argument<HumanizeType>("type", "Humanize type");
        _textArgument = new Argument<string>("text", "Text for humanize");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("humanize",
            "Humanizer text (Pascal-case, Camel-case, Kebab, Underscore, lowercase, etc)");
        command.AddArgument(_humanizeType);
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _humanizeType);

        return command;
    }

    private void CalculateText(string text, HumanizeType humanizeType)
    {
        _humanizeService.CalculateText(text, humanizeType);
    }
}