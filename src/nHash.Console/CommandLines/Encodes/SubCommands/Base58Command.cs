using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base58Command : IBase58Command
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    private readonly IBase58Service _base58Service;
    private readonly IOutputProvider _outputProvider;

    public Base58Command(IBase58Service base58Service, IOutputProvider outputProvider)
    {
        _base58Service = base58Service;
        _outputProvider = outputProvider;
        _decodeText = new Option<bool>(name: "--decode", description: "Decode Base58 text");
        _decodeText.AddAlias("-d");
        _textArgument = new Argument<string>("text", "text for encode/decode Base58");
    }

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base58", "Encode/Decode Base58", GetExamples())
        {
            _decodeText
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateTextHash, _textArgument, _decodeText);
        command.AddAlias("b58");
        
        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples()
    {
        return new List<KeyValuePair<string,string>>()
        {
            new( "Encode a text string in Base58", "nhash encode base85 \"Hello, World\""  ),
            new( "Decode a Base58-encoded string", "nhash encode base85 \"StV1DL6CwTryKyV\" -d" ),
            new( "Decode a Base58-encoded string", "nhash e b85 \"StV1DL6CwTryKyV\" -d" )
        };
    }

    private void CalculateTextHash(string text, bool decode)
    {
        var returnText = _base58Service.Calculate(text, decode);
        _outputProvider.Append(returnText);
    }
}