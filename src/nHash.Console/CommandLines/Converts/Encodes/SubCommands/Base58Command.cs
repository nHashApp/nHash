using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base58Command(IBase58Service base58Service, IOutputProvider outputProvider) : IBase58Command
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Base58 text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "text for encode/decode Base58" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base58", "Encode/Decode Base58", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateTextHash(text ?? string.Empty, decode);
        });
        command.Aliases.Add("b58");
        
        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new( "Encode a text string in Base58", "nhash encode base85 \"Hello, World\""  ),
            new( "Decode a Base58-encoded string", "nhash encode base85 \"StV1DL6CwTryKyV\" -d" ),
            new( "Decode a Base58-encoded string", "nhash e b85 \"StV1DL6CwTryKyV\" -d" )
        ];

    private void CalculateTextHash(string text, bool decode)
    {
        var returnText = base58Service.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}