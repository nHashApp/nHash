using nHash.Application.Abstraction;

namespace nHash.Application.Encodes;

public class Base64Service : IBase64Service 
{
    private readonly IOutputProvider _outputProvider;

    public Base64Service(IOutputProvider outputProvider)
    {
        _outputProvider = outputProvider;
    }

    public void CalculateTextHash(string text, bool decode)
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