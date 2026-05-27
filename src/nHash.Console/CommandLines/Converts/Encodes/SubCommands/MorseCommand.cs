using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class MorseCommand(IMorseService morseService, IOutputProvider outputProvider) : IMorseCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Morse code to text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "Text for encode/decode Morse code" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("morse", "Encode/Decode Morse code", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateText(text ?? string.Empty, decode);
        });
        command.Aliases.Add("ms");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Encode text to Morse code", "nhash encode morse \"Hello World\""),
            new("Decode Morse code to text", "nhash encode morse \".... . .-.. .-.. --- / .-- --- .-. .-.. -..\" -d"),
            new("Using alias", "nhash e ms \"SOS\""),
        ];

    private void CalculateText(string text, bool decode)
    {
        var returnText = morseService.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}
