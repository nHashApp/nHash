using nHash.Application.Encodes;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base64Command : IBase64Command 
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    private readonly IBase64Service _base64Service;
    private readonly IOutputProvider _outputProvider;
    public Base64Command(IBase64Service base64Service, IOutputProvider outputProvider)
    {
        _base64Service = base64Service;
        _outputProvider = outputProvider;
        _decodeText = new Option<bool>(name: "--decode", description: "Decode Base64 text");
        _textArgument = new Argument<string>("text", "text for encode/decode Base64");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("base64", "Encode/Decode Base64")
        {
            _decodeText
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateTextHash, _textArgument, _decodeText);

        return command;
    }

    private void CalculateTextHash(string text, bool decode)
    {
        var returnText=_base64Service.CalculateTextHash(text, decode);
        _outputProvider.Append(returnText);
    }

}