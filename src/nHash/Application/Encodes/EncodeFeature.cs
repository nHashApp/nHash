namespace nHash.Application.Encodes;

public class EncodeFeature : IEncodeFeature, IFeature 
{
    public Command Command => GetCommand();

    private static Command GetCommand()
    {
        var features = new List<IFeature>()
        {
            new JwtTokenFeature(),
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