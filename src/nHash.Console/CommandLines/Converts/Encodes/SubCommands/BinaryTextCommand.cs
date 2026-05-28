using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class BinaryTextCommand(IBinaryTextService binaryTextService, IOutputProvider outputProvider) : IBinaryTextCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode binary/octal/decimal representation to text" };
    private readonly Option<int> _base = new("--base", "-b") { Description = "Numeric base: 2 (binary), 8 (octal), 10 (decimal)", DefaultValueFactory = _ => 2 };
    private readonly Argument<string> _textArgument = new("text") { Description = "Text to encode, or space-separated numbers to decode" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("bintext", "Convert text to/from binary, octal, or decimal representation", GetExamples());
        command.Options.Add(_decodeText);
        command.Options.Add(_base);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            var numericBase = parseResult.GetValue(_base);
            CalculateText(text ?? string.Empty, decode, numericBase);
        });
        command.Aliases.Add("bt");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Encode text to binary", "nhash encode bintext \"Hi\""),
            new("Encode text to octal", "nhash encode bintext \"Hi\" -b 8"),
            new("Encode text to decimal", "nhash encode bintext \"Hi\" -b 10"),
            new("Decode binary back to text", "nhash encode bintext \"01001000 01101001\" -d"),
            new("Using alias", "nhash e bt \"Hi\""),
        ];

    private void CalculateText(string text, bool decode, int numericBase)
    {
        var returnText = binaryTextService.Calculate(text, decode, numericBase);
        outputProvider.Append(returnText);
    }
}
