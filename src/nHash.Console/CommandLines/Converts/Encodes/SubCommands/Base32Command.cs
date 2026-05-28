using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base32Command(IBase32Service base32Service, IOutputProvider outputProvider) : IBase32Command 
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Base32 text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "text for encode/decode Base32" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base32", "Encode/Decode Base32", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateText(text ?? string.Empty, decode);
        });
        command.Aliases.Add("b32");
        
        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new("Encode a text string in Base32", "nhash encode base32 \"Hello, World\""),
            new("Decode a Base32-encoded string", "nhash encode base32 JBSWY3DPEBLW64TBNQ====== -d")
        ];
    
    private void CalculateText(string text, bool decode)
    {
        var returnText = base32Service.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}
