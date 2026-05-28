using System.Text;

namespace nHash.Application.Encodes;

public class Base91Service : IBase91Service
{
    // Base91 alphabet (91 characters)
    private const string Alphabet =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!#$%&()*+,./:;<=>?@[]^_'{|}~\"";

    public string Calculate(string text, bool decode)
    {
        return decode ? Decode(text) : Encode(text);
    }

    private static string Encode(string text)
    {
        var data = Encoding.UTF8.GetBytes(text);
        var result = new StringBuilder();

        var b = 0;
        var n = 0;

        foreach (var t in data)
        {
            b |= t << n;
            n += 8;
            if (n <= 13) continue;

            var v = b & 8191;
            if (v > 88)
            {
                b >>= 13;
                n -= 13;
            }
            else
            {
                v = b & 16383;
                b >>= 14;
                n -= 14;
            }

            result.Append(Alphabet[v % 91]);
            result.Append(Alphabet[v / 91]);
        }

        if (n == 0) return result.ToString();

        result.Append(Alphabet[b % 91]);
        if (n > 7 || b > 90)
            result.Append(Alphabet[b / 91]);

        return result.ToString();
    }

    private static string Decode(string encodedText)
    {
        var result = new List<byte>();

        var v = -1;
        var b = 0;
        var n = 0;

        foreach (var c in encodedText)
        {
            var p = Alphabet.IndexOf(c);
            if (p < 0) continue; // skip unknown chars

            if (v < 0)
            {
                v = p;
            }
            else
            {
                v += p * 91;
                b |= v << n;
                n += (v & 8191) > 88 ? 13 : 14;
                v = -1;

                do
                {
                    result.Add((byte)(b & 255));
                    b >>= 8;
                    n -= 8;
                } while (n > 7);
            }
        }

        if (v > -1)
            result.Add((byte)((b | v << n) & 255));

        return Encoding.UTF8.GetString(result.ToArray());
    }
}
