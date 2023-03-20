namespace nHash.Application.Encodes;

public class Base64Feature : IBase64Feature 
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;
    
    private readonly IOutputProvider _outputProvider;

    public Base64Feature(IOutputProvider outputProvider)
    {
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
        var resultText = !decode
            ? Base64Encode(text)
            : Base64Decode(text);


        _outputProvider.Append(resultText);
    }

    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    private static string Base64Decode(string encodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(encodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}