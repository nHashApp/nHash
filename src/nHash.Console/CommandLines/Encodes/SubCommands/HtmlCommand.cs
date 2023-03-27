using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class HtmlCommand : IHtmlCommand 
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    private readonly IHtmlService _htmlService;
    private readonly IOutputProvider _outputProvider;

    public HtmlCommand(IHtmlService htmlService, IOutputProvider outputProvider)
    {
        _htmlService = htmlService;
        _outputProvider = outputProvider;
        _decodeText = new Option<bool>(name: "--decode", description: "Decode html-encoded text");
        _decodeText.AddAlias("-d");
        _textArgument = new Argument<string>("text", "text for html encode/decode");
    }

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("html", "HTML Encode/Decode", GetExamples())
        {
            _decodeText
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateTextHash, _textArgument, _decodeText);
        command.AddAlias("h");

        return command;
    }
    
    private static List<KeyValuePair<string,string>> GetExamples()
    {
        return new List<KeyValuePair<string,string>>()
        {
            new( "Encode HTML", "nhash encode html \"<h1>Hello World</h1>\""  ),
            new( "Decode HTML", "nhash encode html \"&lt;h1&gt;Hello World&lt;/h1&gt;\" -d" ),
            new( "Decode HTML", "nhash e h \"&lt;h1&gt;Hello World&lt;/h1&gt;\" -d" ),
        };
    }

    private void CalculateTextHash(string text, bool decode)
    {
        var returnText= _htmlService.CalculateTextHash(text, decode);
        _outputProvider.Append(returnText);
    }

    
}