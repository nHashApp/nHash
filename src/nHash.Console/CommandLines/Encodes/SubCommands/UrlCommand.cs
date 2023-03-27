using nHash.Application.Encodes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class UrlCommand : IUrlCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    private readonly IUrlService _urlService;
    private readonly IOutputProvider _outputProvider;

    public UrlCommand(IUrlService urlService, IOutputProvider outputProvider)
    {
        _urlService = urlService;
        _outputProvider = outputProvider;
        _decodeText = new Option<bool>(name: "--decode", description: "Decode url-encoded text");
        _decodeText.AddAlias("-d");
        _textArgument = new Argument<string>("text", "text for url encode/decode");
    }

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("url", "URL Encode/Decode", GetExamples())
        {
            _decodeText
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateTextHash, _textArgument, _decodeText);
        command.AddAlias("u");

        return command;
    }

    private static List<KeyValuePair<string,string>> GetExamples()
    {
        return new List<KeyValuePair<string,string>>()
        {
            new( "URL encoding", "nhash encode url \"Hello World\""  ),
            new( "URL decoding", "nhash encode url \"Hello%20World%21\" -d" ),
            new( "URL decoding", "nhash e u \"Hello%20World%21\" -d" ),
        };
    }
    
    private void CalculateTextHash(string text, bool decode)
    {
        var returnText= _urlService.CalculateTextHash(text, decode);
        _outputProvider.Append(returnText);

    }
}