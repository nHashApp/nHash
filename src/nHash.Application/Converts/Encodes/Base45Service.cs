using System.Text;

namespace nHash.Application.Encodes;

public class Base45Service : IBase45Service
{
    // RFC 9285 Base45 alphabet (45 characters, index 0-44)
    private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";

    public string Calculate(string text, bool decode)
    {
        return decode ? Decode(text) : Encode(text);
    }

    private static string Encode(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        var result = new StringBuilder();

        for (var i = 0; i < bytes.Length; i += 2)
        {
            if (i + 1 < bytes.Length)
            {
                // Encode two bytes as three base-45 digits
                var n = bytes[i] * 256 + bytes[i + 1];
                var c = n % 45;
                n /= 45;
                var b = n % 45;
                n /= 45;
                var a = n;
                result.Append(Alphabet[c]);
                result.Append(Alphabet[b]);
                result.Append(Alphabet[a]);
            }
            else
            {
                // Encode one remaining byte as two base-45 digits
                var n = bytes[i];
                var b = n % 45;
                n /= 45;
                var a = n;
                result.Append(Alphabet[b]);
                result.Append(Alphabet[a]);
            }
        }

        return result.ToString();
    }

    private static string Decode(string encodedText)
    {
        var text = encodedText.Trim();
        var result = new List<byte>();

        for (var i = 0; i < text.Length; i += 3)
        {
            if (i + 2 < text.Length)
            {
                // Decode three base-45 digits into two bytes
                var c = Alphabet.IndexOf(text[i]);
                var b = Alphabet.IndexOf(text[i + 1]);
                var a = Alphabet.IndexOf(text[i + 2]);

                if (c < 0 || b < 0 || a < 0)
                    throw new FormatException($"Invalid Base45 character at position {i}.");

                var n = a * 45 * 45 + b * 45 + c;
                result.Add((byte)(n / 256));
                result.Add((byte)(n % 256));
            }
            else if (i + 1 < text.Length)
            {
                // Decode two base-45 digits into one byte
                var b = Alphabet.IndexOf(text[i]);
                var a = Alphabet.IndexOf(text[i + 1]);

                if (b < 0 || a < 0)
                    throw new FormatException($"Invalid Base45 character at position {i}.");

                var n = a * 45 + b;
                result.Add((byte)n);
            }
        }

        return Encoding.UTF8.GetString(result.ToArray());
    }
}
