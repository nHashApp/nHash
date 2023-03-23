using nHash.Application.Encodes;

namespace nHash.Console.CommandLines.Encodes;

public class EncodeCommand : IEncodeFeature
{
    public Command Command => GetCommand();

    private readonly IJwtTokenCommand _jwtTokenFeature;
    private readonly IBase64Command _base64Feature;
    private readonly IUrlCommand _urlFeature;
    private readonly IHtmlCommand _htmlFeature;
    
    public EncodeCommand(IJwtTokenCommand jwtTokenFeature, IBase64Command base64Feature, IUrlCommand urlFeature,
        IHtmlCommand htmlFeature)
    {
        _jwtTokenFeature = jwtTokenFeature;
        _base64Feature = base64Feature;
        _urlFeature = urlFeature;
        _htmlFeature = htmlFeature;
    }

    private Command GetCommand()
    {
        var features = new List<IFeature>()
        {
            _jwtTokenFeature,
            _base64Feature,
            _urlFeature,
            _htmlFeature,
        };

        var command = new Command("encode", "Encode/Decode features (JWT, Base64, URL, HTML)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }
}