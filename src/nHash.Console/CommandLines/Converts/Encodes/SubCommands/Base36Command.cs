using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base36Command(IBase36Service base36Service, IOutputProvider outputProvider) : IBase36Command 
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Base36 text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "text for encode/decode Base36" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base36", "Encode/Decode Base36", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateText(text ?? string.Empty, decode);
        });
        command.Aliases.Add("b36");
        
        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new("Encode a text string in Base36", "nhash encode base36 \"Hello\""),
            new("Decode a Base36-encoded string", "nhash encode base36 4s02c3d -d")
        ];
    
    private void CalculateText(string text, bool decode)
    {
        var returnText = base36Service.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}
