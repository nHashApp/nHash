using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base64Command : IBase64Command 
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    private readonly IBase64Service _base64Service;
    private readonly IOutputProvider _outputProvider;
    public Base64Command(IBase64Service base64Service, IOutputProvider outputProvider)
    {
        _base64Service = base64Service;
        _outputProvider = outputProvider;
        _decodeText = new Option<bool>(name: "--decode", description: "Decode Base64 text");
        _decodeText.AddAlias("-d");
        _textArgument = new Argument<string>("text", "text for encode/decode Base64");
    }

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base64", "Encode/Decode Base64", GetExamples())
        {
            _decodeText
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateTextHash, _textArgument, _decodeText);
        command.AddAlias("b64");
        
        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples()
    {
        return new List<KeyValuePair<string,string>>()
        {
            new( "Encode a text string in Base64", "nhash encode base64 \"Hello, World\""  ),
            new( "Decode a Base64-encoded string", "nhash encode base64 SGVsbG8sIFdvcmxkIQ== -d" ),
            new( "Decode a Base64-encoded string", "nhash e b64 SGVsbG8sIFdvcmxkIQ== -d" )
        };
    }
    
    private void CalculateTextHash(string text, bool decode)
    {
        var returnText=_base64Service.Calculate(text, decode);
        _outputProvider.Append(returnText);
    }

}