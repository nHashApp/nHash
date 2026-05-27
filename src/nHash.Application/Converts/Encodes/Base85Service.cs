using System.Text;

namespace nHash.Application.Encodes;

public class Base85Service : IBase85Service
{
    private const string Prefix = "<~";
    private const string Suffix = "~>";

    public string Calculate(string text, bool decode)
    {
        return !decode ? Base85Encode(text) : Base85Decode(text);
    }

    private static string Base85Encode(string plainText)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);
        if (bytes.Length == 0) return string.Empty;

        var sb = new StringBuilder();
        sb.Append(Prefix);

        int count = 0;
        uint block = 0;

        foreach (var b in bytes)
        {
            block = (block << 8) | b;
            count++;

            if (count == 4)
            {
                if (block == 0)
                {
                    sb.Append('z');
                }
                else
                {
                    EncodeBlock(sb, block, 5);
                }
                block = 0;
                count = 0;
            }
        }

        if (count > 0)
        {
            int padding = 4 - count;
            block <<= (padding * 8);
            EncodeBlock(sb, block, count + 1);
        }

        sb.Append(Suffix);
        return sb.ToString();
    }

    private static void EncodeBlock(StringBuilder sb, uint block, int charsToWrite)
    {
        char[] chars = new char[5];
        for (int i = 4; i >= 0; i--)
        {
            chars[i] = (char)((block % 85) + 33);
            block /= 85;
        }
        for (int i = 0; i < charsToWrite; i++)
        {
            sb.Append(chars[i]);
        }
    }

    private static string Base85Decode(string encodedData)
    {
        var s = encodedData.Trim();
        if (s.StartsWith(Prefix)) s = s[Prefix.Length..];
        if (s.EndsWith(Suffix)) s = s[..^Suffix.Length];

        var bytes = new List<byte>();
        int count = 0;
        uint block = 0;

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (char.IsWhiteSpace(c)) continue;

            if (c == 'z' && count == 0)
            {
                bytes.AddRange([0, 0, 0, 0]);
                continue;
            }

            if (c < 33 || c > 117) continue;

            block = (block * 85) + (uint)(c - 33);
            count++;

            if (count == 5)
            {
                bytes.Add((byte)((block >> 24) & 0xFF));
                bytes.Add((byte)((block >> 16) & 0xFF));
                bytes.Add((byte)((block >> 8) & 0xFF));
                bytes.Add((byte)(block & 0xFF));
                block = 0;
                count = 0;
            }
        }

        if (count > 0)
        {
            int padding = 5 - count;
            for (int i = 0; i < padding; i++)
            {
                block = (block * 85) + 84;
            }
            for (int i = 0; i < count - 1; i++)
            {
                bytes.Add((byte)((block >> (24 - (i * 8))) & 0xFF));
            }
        }

        return Encoding.UTF8.GetString([.. bytes]);
    }
}
