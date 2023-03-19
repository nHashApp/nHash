using System.Web;

namespace nHash.Application.Encodes;

public class UrlFeature : IUrlFeature
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    public UrlFeature()
    {
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

    private static void CalculateTextHash(string text, bool decode)
    {
        var resultText = !decode
            ? UrlEncode(text)
            : UrlDecode(text);

        Console.WriteLine(resultText);
    }

    private static string UrlEncode(string plainText)
    {
        return HttpUtility.UrlEncode(plainText);
    }

    private static string UrlDecode(string encodedData)
    {
        return HttpUtility.UrlDecode(encodedData);
    }
}