using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base64Command(IBase64Service base64Service, IOutputProvider outputProvider) : IBase64Command 
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Base64 text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "text for encode/decode Base64" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base64", "Encode/Decode Base64", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateTextHash(text ?? string.Empty, decode);
        });
        command.Aliases.Add("b64");
        
        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new( "Encode a text string in Base64", "nhash encode base64 \"Hello, World\""  ),
            new( "Decode a Base64-encoded string", "nhash encode base64 SGVsbG8sIFdvcmxkIQ== -d" ),
            new( "Decode a Base64-encoded string", "nhash e b64 SGVsbG8sIFdvcmxkIQ== -d" )
        ];
    
    private void CalculateTextHash(string text, bool decode)
    {
        var returnText = base64Service.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}