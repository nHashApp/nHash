using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class UrlCommand(IUrlService urlService, IOutputProvider outputProvider) : IUrlCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode url-encoded text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "text for url encode/decode" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("url", "URL Encode/Decode", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateTextHash(text ?? string.Empty, decode);
        });
        command.Aliases.Add("u");

        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new( "URL encoding", "nhash encode url \"Hello World\""  ),
            new( "URL decoding", "nhash encode url \"Hello%20World%21\" -d" ),
            new( "URL decoding", "nhash e u \"Hello%20World%21\" -d" ),
        ];
    
    private void CalculateTextHash(string text, bool decode)
    {
        var returnText = urlService.CalculateTextHash(text, decode);
        outputProvider.Append(returnText);
    }
}