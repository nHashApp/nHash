using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Json;

namespace nHash.Application.Texts;

public class TextFeature : ITextFeature
{
    public Command Command => GetCommand();

    private readonly IHumanizeFeature _humanizeFeature;
    private readonly IJsonFeature _jsonFeature;

    public TextFeature(IHumanizeFeature humanizeFeature, IJsonFeature jsonFeature)
    {
        _humanizeFeature = humanizeFeature;
        _jsonFeature = jsonFeature;
    }

    private Command GetCommand()
    {
        var features = new List<IFeature>()
        {
            _humanizeFeature,
            _jsonFeature,
        };

        var command = new Command("text", "Text utilities (Humanizer)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }
}