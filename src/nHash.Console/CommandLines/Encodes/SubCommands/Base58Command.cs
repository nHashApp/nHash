using nHash.Application.Encodes;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class Base58Command : IBase58Command
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    private readonly IBase58Service _base58Service;
    private readonly IOutputProvider _outputProvider;
    public Base58Command(IBase58Service base58Service, IOutputProvider outputProvider)
    {
        _base58Service = base58Service;
        _outputProvider = outputProvider;
        _decodeText = new Option<bool>(name: "--decode", description: "Decode Base58 text");
        _decodeText.AddAlias("-d");
        _textArgument = new Argument<string>("text", "text for encode/decode Base58");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("base58", "Encode/Decode Base64")
        {
            _decodeText
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateTextHash, _textArgument, _decodeText);

        return command;
    }

    private void CalculateTextHash(string text, bool decode)
    {
        var returnText=_base58Service.Calculate(text, decode);
        _outputProvider.Append(returnText);
    }

}