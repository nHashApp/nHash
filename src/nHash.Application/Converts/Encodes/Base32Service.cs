using System.Text;

namespace nHash.Application.Encodes;

public class Base32Service : IBase32Service
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    public string Calculate(string text, bool decode)
    {
        return !decode ? Base32Encode(text) : Base32Decode(text);
    }

    private static string Base32Encode(string plainText)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);
        if (bytes.Length == 0) return string.Empty;

        var sb = new StringBuilder((bytes.Length + 4) / 5 * 8);
        int bitCount = 0;
        int currentByte = 0;
        
        foreach (var b in bytes)
        {
            currentByte = (currentByte << 8) | b;
            bitCount += 8;
            while (bitCount >= 5)
            {
                sb.Append(Alphabet[(currentByte >> (bitCount - 5)) & 0x1F]);
                bitCount -= 5;
            }
        }

        if (bitCount > 0)
        {
            sb.Append(Alphabet[(currentByte << (5 - bitCount)) & 0x1F]);
        }

        while (sb.Length % 8 != 0)
        {
            sb.Append('=');
        }

        return sb.ToString();
    }

    private static string Base32Decode(string encodedData)
    {
        var cleanData = encodedData.TrimEnd('=').ToUpperInvariant();
        if (cleanData.Length == 0) return string.Empty;

        var bytes = new List<byte>((cleanData.Length * 5) / 8);
        int bitCount = 0;
        int currentByte = 0;

        foreach (var c in cleanData)
        {
            int index = Alphabet.IndexOf(c);
            if (index < 0) continue;

            currentByte = (currentByte << 5) | index;
            bitCount += 5;
            while (bitCount >= 8)
            {
                bytes.Add((byte)((currentByte >> (bitCount - 8)) & 0xFF));
                bitCount -= 8;
            }
        }

        return Encoding.UTF8.GetString([.. bytes]);
    }
}
