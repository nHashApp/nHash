using Humanizer;
using nHash.Application.Abstraction;
using nHash.Application.Texts.Humanizers.Models;

namespace nHash.Application.Texts.Humanizers;

public class HumanizeService : IHumanizeService
{
    public string CalculateText(string text, HumanizeType humanizeType)
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

        return resultText;
    }
}