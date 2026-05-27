using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Texts.SubCommands;

namespace nHash.Console.CommandLines.Texts;

public class TextCommand(
    IHumanizeCommand humanizeFeature,
    IJsonCommand jsonFeature,
    IYamlCommand yamlFeature,
    IXmlCommand xmlFeature,
    ICaseCommand caseFeature,
    IDiffCommand diffFeature,
    IStatsCommand statsFeature,
    ILoremCommand loremFeature)
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
            xmlFeature,
            caseFeature,
            diffFeature,
            statsFeature,
            loremFeature
        ];

        var command = new BaseCommand("text", "Text utilities (Humanizer, JSON, YAML, XML, Case, Diff, Stats, Lorem)");
        command.Aliases.Add("t");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}