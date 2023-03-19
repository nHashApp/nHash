using System.Web;

namespace nHash.Application.SubFeatures.Encodes;

public class HtmlFeature : IFeature
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _decodeText;
    private readonly Argument<string> _textArgument;

    public HtmlFeature()
    {
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

    private static void CalculateTextHash(string text, bool decode)
    {
        var resultText = !decode
            ? UrlEncode(text)
            : UrlDecode(text);

        Console.WriteLine(resultText);
    }

    private static string UrlEncode(string plainText)
    {
        return HttpUtility.HtmlEncode(plainText);
    }

    private static string UrlDecode(string encodedData)
    {
        return HttpUtility.HtmlDecode(encodedData);
    }
}