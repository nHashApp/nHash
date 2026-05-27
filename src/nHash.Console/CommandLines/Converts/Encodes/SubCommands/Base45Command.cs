using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base45Command(IBase45Service base45Service, IOutputProvider outputProvider) : IBase45Command
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Base45 text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "Text for encode/decode Base45" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base45", "Encode/Decode Base45 (RFC 9285)", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateText(text ?? string.Empty, decode);
        });
        command.Aliases.Add("b45");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Encode a text string in Base45", "nhash encode base45 \"Hello World\""),
            new("Decode a Base45-encoded string", "nhash encode base45 \"%69 VD82EI2+.D\" -d"),
            new("Decode using alias", "nhash e b45 \"%69 VD82EI2+.D\" -d"),
        ];

    private void CalculateText(string text, bool decode)
    {
        var returnText = base45Service.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}
