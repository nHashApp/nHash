using System.Text;

namespace nHash.Application.Encodes;

public class HexService : IHexService
{
    public string Calculate(string text, bool decode)
    {
        return !decode ? HexEncode(text) : HexDecode(text);
    }

    private static string HexEncode(string plainText)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToHexString(bytes);
    }

    private static string HexDecode(string encodedData)
    {
        var cleanData = encodedData.Trim().Replace(" ", "").Replace("-", "");
        var bytes = Convert.FromHexString(cleanData);
        return Encoding.UTF8.GetString(bytes);
    }
}
