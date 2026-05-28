using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base62Command(IBase62Service base62Service, IOutputProvider outputProvider) : IBase62Command 
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Base62 text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "text for encode/decode Base62" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base62", "Encode/Decode Base62", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateText(text ?? string.Empty, decode);
        });
        command.Aliases.Add("b62");
        
        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new("Encode a text string in Base62", "nhash encode base62 \"Hello\""),
            new("Decode a Base62-encoded string", "nhash encode base62 2tq5Zp -d")
        ];
    
    private void CalculateText(string text, bool decode)
    {
        var returnText = base62Service.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}
