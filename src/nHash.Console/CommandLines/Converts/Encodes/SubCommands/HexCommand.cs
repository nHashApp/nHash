using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class HexCommand(IHexService hexService, IOutputProvider outputProvider) : IHexCommand 
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Hex text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "text for encode/decode Hex" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("hex", "Encode/Decode Hex (Base16)", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateText(text ?? string.Empty, decode);
        });
        command.Aliases.Add("b16");
        
        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new("Encode a text string in Hex", "nhash encode hex \"Hello\""),
            new("Decode a Hex-encoded string", "nhash encode hex 48656C6C6F -d")
        ];
    
    private void CalculateText(string text, bool decode)
    {
        var returnText = hexService.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}
