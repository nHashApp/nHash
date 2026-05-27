using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Texts.SubCommands;

namespace nHash.Console.CommandLines.Texts;

public class TextCommand(
    IHumanizeCommand humanizeFeature,
    IJsonCommand jsonFeature,
    IYamlCommand yamlFeature,
    IXmlCommand xmlFeature)
    : ITextCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        List<IFeature> features =
        [
            humanizeFeature,
            jsonFeature,
            yamlFeature,
            xmlFeature
        ];

        var command = new BaseCommand("text", "Text utilities (Humanizer, JSON, YAML, XML)");
        command.Aliases.Add("t");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}