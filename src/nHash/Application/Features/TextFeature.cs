using nHash.Application.SubFeatures.Texts;

namespace nHash.Application.Features;

public class TextFeature : IFeature
{
    public Command Command => GetCommand();

    private static Command GetCommand()
    {
        var features = new List<IFeature>()
        {
            new HumanizeFeature(),
            new JsonFeature(),
        };
        
        var command = new Command("text", "Text utilities (Humanizer)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }
        return command;
    }
}