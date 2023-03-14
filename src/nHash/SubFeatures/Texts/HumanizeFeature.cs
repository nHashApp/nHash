using Humanizer;
using nHash.Features.Models;

namespace nHash.SubFeatures.Texts;

public class HumanizeFeature : IFeature
{
    public Command Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument;
    private readonly Argument<HumanizeType> _humanizeType;

    public HumanizeFeature()
    {
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

    private static void CalculateText(string text, HumanizeType humanizeType)
    {
        var resultText = humanizeType switch
        {
            HumanizeType.Humanize => text.Humanize(),
            HumanizeType.Dehumanize => text.Dehumanize(),
            HumanizeType.Pascal => text.Pascalize(),
            HumanizeType.Camel => text.Camelize(),
            HumanizeType.Kebab => text.Kebaberize(),
            HumanizeType.Underscore => text.Underscore(),
            HumanizeType.Hyphenate => text.Hyphenate(),
            HumanizeType.Lowercase => text.ToLower(),
            HumanizeType.Uppercase => text.ToUpper(),
            _ => text
        };

        Console.WriteLine(resultText);
    }
}