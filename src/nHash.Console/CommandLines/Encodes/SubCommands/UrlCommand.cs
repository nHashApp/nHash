using nHash.Application.Encodes;
using nHash.Console.CommandLines.Encodes.SubCommands;

namespace nHash.Console.CommandLines.Encodes;

public class UrlCommand : IUrlCommand
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    private readonly IUrlService _urlService;
    private readonly IOutputProvider _outputProvider;

    public UrlCommand(IUrlService urlService, IOutputProvider outputProvider)
    {
        _urlService = urlService;
        _outputProvider = outputProvider;
        _decodeText = new Option<bool>(name: "--decode", description: "Decode url-encoded text");
        _textArgument = new Argument<string>("text", "text for url encode/decode");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("url", "URL Encode/Decode")
        {
            _decodeText
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateTextHash, _textArgument, _decodeText);

        return command;
    }

    private void CalculateTextHash(string text, bool decode)
    {
        var returnText= _urlService.CalculateTextHash(text, decode);
        _outputProvider.Append(returnText);

    }
}