using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base91Command(IBase91Service base91Service, IOutputProvider outputProvider) : IBase91Command
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode Base91 text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "Text for encode/decode Base91" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("base91", "Encode/Decode Base91", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateText(text ?? string.Empty, decode);
        });
        command.Aliases.Add("b91");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Encode a text string in Base91", "nhash encode base91 \"Hello World\""),
            new("Decode a Base91-encoded string", "nhash encode base91 \"ToYU,8\">%GA\" -d"),
            new("Decode using alias", "nhash e b91 \"ToYU,8\">%GA\" -d"),
        ];

    private void CalculateText(string text, bool decode)
    {
        var returnText = base91Service.Calculate(text, decode);
        outputProvider.Append(returnText);
    }
}
