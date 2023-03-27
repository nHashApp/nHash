namespace nHash.Application.Encodes;

public class Base64Service : IBase64Service 
{
    public string Calculate(string text, bool decode)
    {
        var resultText = !decode
            ? Base64Encode(text)
            : Base64Decode(text);

        return resultText;
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