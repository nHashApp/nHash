using nHash.Application.Encodes;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class HtmlCommand : IHtmlCommand 
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    private readonly IHtmlService _htmlService;
    
    public HtmlCommand(IHtmlService htmlService)
    {
        _htmlService = htmlService;
        _decodeText = new Option<bool>(name: "--decode", description: "Decode html-encoded text");
        _textArgument = new Argument<string>("text", "text for html encode/decode");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("html", "HTML Encode/Decode")
        {
            _decodeText
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateTextHash, _textArgument, _decodeText);

        return command;
    }

    private void CalculateTextHash(string text, bool decode)
    {
        _htmlService.CalculateTextHash(text, decode);
    }

    
}