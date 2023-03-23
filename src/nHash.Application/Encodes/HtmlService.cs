using System.Web;
using nHash.Application.Abstraction;

namespace nHash.Application.Encodes;

public class HtmlService : IHtmlService
{
    public string CalculateTextHash(string text, bool decode)
    {
        var resultText = !decode
            ? UrlEncode(text)
            : UrlDecode(text);

        return resultText;
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