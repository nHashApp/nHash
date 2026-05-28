using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base85Command(IBase85Service base85Service, IOutputProvider outputProvider) : IBase85Command 
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Base85 text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "text for encode/decode Base85" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base85", "Encode/Decode Base85 (Ascii85)", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateText(text ?? string.Empty, decode);
        });
        command.Aliases.Add("b85");
        
        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new("Encode a text string in Base85", "nhash encode base85 \"Hello\""),
            new("Decode a Base85-encoded string", "nhash encode base85 \"<~87cURD]i~>\" -d")
        ];
    
    private void CalculateText(string text, bool decode)
    {
        var returnText = base85Service.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}
