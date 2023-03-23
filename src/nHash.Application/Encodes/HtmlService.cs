using System.Web;
using nHash.Application.Abstraction;

namespace nHash.Application.Encodes;

public class HtmlService : IHtmlService
{
    private readonly IOutputProvider _outputProvider;
    public HtmlService(IOutputProvider outputProvider)
    {
        _outputProvider = outputProvider;
    }

    public void CalculateTextHash(string text, bool decode)
    {
        var resultText = !decode
            ? UrlEncode(text)
            : UrlDecode(text);

        _outputProvider.Append(resultText);
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