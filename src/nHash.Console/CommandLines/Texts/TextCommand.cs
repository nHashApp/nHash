using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Texts.SubCommands;

namespace nHash.Console.CommandLines.Texts;

public class TextCommand : ITextCommand
{
    public BaseCommand Command => GetCommand();

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

    private BaseCommand GetCommand()
    {
        var features = new List<IFeature>()
        {
            _humanizeFeature,
            _jsonFeature,
            _yamlFeature,
            _xmlFeature
        };

        var command = new BaseCommand("text", "Text utilities (Humanizer, JSON, YAML, XML)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }
}