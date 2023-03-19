using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Json;

namespace nHash.Application.Texts;

public class TextFeature : ITextFeature, IFeature 
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