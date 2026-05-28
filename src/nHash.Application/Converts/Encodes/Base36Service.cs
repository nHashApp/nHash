using System.Numerics;
using System.Text;

namespace nHash.Application.Encodes;

public class Base36Service : IBase36Service
{
    private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";

    public string Calculate(string text, bool decode)
    {
        return !decode ? Base36Encode(text) : Base36Decode(text);
    }

    private static string Base36Encode(string plainText)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);
        if (bytes.Length == 0) return string.Empty;

        var bigIntBytes = new byte[bytes.Length + 1];
        Array.Copy(bytes, bigIntBytes, bytes.Length);
        var bigInt = new BigInteger(bigIntBytes);

        var sb = new StringBuilder();
        while (bigInt > 0)
        {
            bigInt = BigInteger.DivRem(bigInt, 36, out var remainder);
            sb.Append(Alphabet[(int)remainder]);
        }

        var chars = sb.ToString().ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    private static string Base36Decode(string encodedData)
    {
        var cleanData = encodedData.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(cleanData)) return string.Empty;

        BigInteger bigInt = 0;
        foreach (var c in cleanData)
        {
            int index = Alphabet.IndexOf(c);
            if (index < 0) continue;

            bigInt = (bigInt * 36) + index;
        }

        var bytes = bigInt.ToByteArray();
        int len = bytes.Length;
        while (len > 0 && bytes[len - 1] == 0)
        {
            len--;
        }

        return Encoding.UTF8.GetString(bytes, 0, len);
    }
}
