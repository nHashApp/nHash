using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class HtmlCommand(IHtmlService htmlService, IOutputProvider outputProvider) : IHtmlCommand 
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText = new("--decode", "-d") { Description = "Decode html-encoded text" };
    private readonly Argument<string> _textArgument = new("text") { Description = "text for html encode/decode" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("html", "HTML Encode/Decode", GetExamples());
        command.Options.Add(_decodeText);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var decode = parseResult.GetValue(_decodeText);
            CalculateTextHash(text ?? string.Empty, decode);
        });
        command.Aliases.Add("h");

        return command;
    }
    
    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new( "Encode HTML", "nhash encode html \"<h1>Hello World</h1>\""  ),
            new( "Decode HTML", "nhash encode html \"&lt;h1&gt;Hello World&lt;/h1&gt;\" -d" ),
            new( "Decode HTML", "nhash e h \"&lt;h1&gt;Hello World&lt;/h1&gt;\" -d" ),
        ];

    private void CalculateTextHash(string text, bool decode)
    {
        var returnText = htmlService.CalculateTextHash(text, decode);
        outputProvider.Append(returnText);
    }
}