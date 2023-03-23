using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Json;
using nHash.Application.Texts.Xml;
using nHash.Application.Texts.Yaml;

namespace nHash.Application.Texts;

public class TextFeature : ITextFeature
{
    public Command Command => GetCommand();

    private readonly IHumanizeFeature _humanizeFeature;
    private readonly IJsonFeature _jsonFeature;
    private readonly IYamlFeature _yamlFeature;
    private readonly IXmlFeature _xmlFeature;

    public TextFeature(IHumanizeFeature humanizeFeature, IJsonFeature jsonFeature, IYamlFeature yamlFeature,
        IXmlFeature xmlFeature)
    {
        _humanizeFeature = humanizeFeature;
        _jsonFeature = jsonFeature;
        _yamlFeature = yamlFeature;
        _xmlFeature = xmlFeature;
    }

    private Command GetCommand()
    {
        var features = new List<IFeature>()
        {
            _humanizeFeature,
            _jsonFeature,
            _yamlFeature,
            _xmlFeature
        };

        var command = new Command("text", "Text utilities (Humanizer)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }
}