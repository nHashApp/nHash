using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class PunycodeCommand(IPunycodeService punycodeService, IOutputProvider outputProvider) : IPunycodeCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Punycode to Unicode" };
    private readonly Argument<string> _textArgument = new("text") { Description = "Text for encode/decode Punycode" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("punycode", "Encode/Decode Punycode (internationalized domain names)", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateText(text ?? string.Empty, decode);
        });
        command.Aliases.Add("pny");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Encode a Unicode domain to Punycode", "nhash encode punycode \"münchen.de\""),
            new("Decode a Punycode domain to Unicode", "nhash encode punycode \"xn--mnchen-3ya.de\" -d"),
            new("Decode using alias", "nhash e pny \"xn--mnchen-3ya.de\" -d"),
        ];

    private void CalculateText(string text, bool decode)
    {
        var returnText = punycodeService.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}
