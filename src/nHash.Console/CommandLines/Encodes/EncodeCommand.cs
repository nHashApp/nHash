using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Encodes.SubCommands;

namespace nHash.Console.CommandLines.Encodes;

public class EncodeCommand : IEncodeCommand
{
    public BaseCommand Command => GetCommand();

    private readonly IJwtTokenCommand _jwtTokenFeature;
    private readonly IBase64Command _base64Feature;
    private readonly IBase58Command _base58Feature;
    private readonly IUrlCommand _urlFeature;
    private readonly IHtmlCommand _htmlFeature;
    
    public EncodeCommand(IJwtTokenCommand jwtTokenFeature, IBase64Command base64Feature, IUrlCommand urlFeature,
        IHtmlCommand htmlFeature, IBase58Command base58Feature)
    {
        _jwtTokenFeature = jwtTokenFeature;
        _base64Feature = base64Feature;
        _urlFeature = urlFeature;
        _htmlFeature = htmlFeature;
        _base58Feature = base58Feature;
    }

    private BaseCommand GetCommand()
    {
        var features = new List<IFeature>()
        {
            _jwtTokenFeature,
            _base64Feature,
            _base58Feature,
            _urlFeature,
            _htmlFeature,
        };

        var command = new BaseCommand("encode", "Encode/Decode features (JWT, Base64, URL, HTML)");
        command.AddAlias("e");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }
    
}