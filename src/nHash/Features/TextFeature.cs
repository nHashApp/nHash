using nHash.SubFeatures.Encodes;
using nHash.SubFeatures.Texts;

namespace nHash.Features;

public class TextFeature : IFeature
{
    public Command Command => GetCommand();

    private static Command GetCommand()
    {
        var features = new List<IFeature>()
        {
            new HumanizeFeature(),
        };
        
        var command = new Command("text", "Text utilities (Humanizer)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }
        return command;
    }
}