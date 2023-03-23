using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Json;
using nHash.Application.Texts.Xml;
using nHash.Application.Texts.Yaml;
using nHash.Console.CommandLines.Texts.Humanizers;
using nHash.Console.CommandLines.Texts.Json;
using nHash.Console.CommandLines.Texts.Xml;
using nHash.Console.CommandLines.Texts.Yaml;

namespace nHash.Console.CommandLines.Texts;

public class TextCommand : ITextCommand
{
    public Command Command => GetCommand();

    private readonly IHumanizeCommand _humanizeFeature;
    private readonly IJsonCommand _jsonFeature;
    private readonly IYamlCommand _yamlFeature;
    private readonly IXmlCommand _xmlFeature;

    public TextCommand(IHumanizeCommand humanizeFeature, IJsonCommand jsonFeature, IYamlCommand yamlFeature,
        IXmlCommand xmlFeature)
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

        var command = new Command("text", "Text utilities (Humanizer, JSON, YAML, XML)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }
}