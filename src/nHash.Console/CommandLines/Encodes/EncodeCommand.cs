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

        var command = new BaseCommand("encode", "Encode/Decode features (JWT, Base64, URL, HTML)", GetExamples());
        command.AddAlias("e");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }
    
    private static List<KeyValuePair<string,string>> GetExamples()
    {
        return new List<KeyValuePair<string,string>>()
        {
            new( "JWT", "nhash encode jwt eyJhbGciOiJIUzI1NiIsInR5..."  ),
            new( "Base64", "nhash encode base64 \"Hello, World\"" ),
            new( "Base64 decode", "nhash e b64 SGVsbG8sIFdvcmxkIQ== -d" ),
            new( "Base58", "nhash encode base58 \"Hello, World\"" ),
            new( "Base58", "nhash e b58 \"Hello, World\"" ),
            new( "URL", "nhash encode url \"https://github.com\"" ),
            new( "HTML", "nhash encode html \"<h1>nHash Encode Command</h1>\"" ),
        };
    }
}