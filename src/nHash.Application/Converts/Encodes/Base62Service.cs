using System.Numerics;
using System.Text;

namespace nHash.Application.Encodes;

public class Base62Service : IBase62Service
{
    private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public string Calculate(string text, bool decode)
    {
        return !decode ? Base62Encode(text) : Base62Decode(text);
    }

    private static string Base62Encode(string plainText)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);
        if (bytes.Length == 0) return string.Empty;

        // Force positive BigInteger by appending a zero byte at the end
        var bigIntBytes = new byte[bytes.Length + 1];
        Array.Copy(bytes, bigIntBytes, bytes.Length);
        var bigInt = new BigInteger(bigIntBytes);

        var sb = new StringBuilder();
        while (bigInt > 0)
        {
            bigInt = BigInteger.DivRem(bigInt, 62, out var remainder);
            sb.Append(Alphabet[(int)remainder]);
        }

        // Reverse to get the correct order
        var chars = sb.ToString().ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    private static string Base62Decode(string encodedData)
    {
        if (string.IsNullOrWhiteSpace(encodedData)) return string.Empty;

        BigInteger bigInt = 0;
        foreach (var c in encodedData)
        {
            int index = Alphabet.IndexOf(c);
            if (index < 0) continue;

            bigInt = (bigInt * 62) + index;
        }

        var bytes = bigInt.ToByteArray();
        
        // Strip trailing zero byte used for forcing positive BigInteger if it exists
        int len = bytes.Length;
        while (len > 0 && bytes[len - 1] == 0)
        {
            len--;
        }

        return Encoding.UTF8.GetString(bytes, 0, len);
    }
}
