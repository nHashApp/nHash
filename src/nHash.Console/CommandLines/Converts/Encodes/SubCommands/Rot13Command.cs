using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Rot13Command(IRot13Service rot13Service, IOutputProvider outputProvider) : IRot13Command
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<int> _shift = new("--shift", "-s") { Description = "Caesar shift amount (1-25, default 13 for ROT13)", DefaultValueFactory = _ => 13 };
    private readonly Argument<string> _textArgument = new("text") { Description = "Text for ROT13/Caesar cipher" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("rot13", "ROT13 / Caesar cipher encode/decode", GetExamples());
        command.Options.Add(_shift);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var shift = parseResult.GetValue(_shift);
            CalculateText(text ?? string.Empty, shift);
        });
        command.Aliases.Add("rot");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("ROT13 encode/decode (symmetric)", "nhash encode rot13 \"Hello World\""),
            new("Caesar cipher with shift 3 (encode)", "nhash encode rot13 \"Hello World\" -s 3"),
            new("Caesar cipher with shift 3 (decode)", "nhash encode rot13 \"Khoor Zruog\" -s -3"),
            new("Using alias", "nhash e rot \"Hello World\""),
        ];

    private void CalculateText(string text, int shift)
    {
        var returnText = rot13Service.Calculate(text, shift);
        outputProvider.Append(returnText);
    }
}
