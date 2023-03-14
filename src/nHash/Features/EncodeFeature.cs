using nHash.SubFeatures.Encodes;

namespace nHash.Features;

public class EncodeFeature : IFeature
{
    public Command Command => GetCommand();

    private static Command GetCommand()
    {
        var features = new List<IFeature>()
        {
            new JwtTokenDecodeFeature(),
            new Base64Feature(),
            new UrlFeature(),
            new HtmlFeature(),
        };
        
        var command = new Command("encode", "Encode/Decode features (JWT, Base64, URL, HTML)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }
        return command;
    }
}