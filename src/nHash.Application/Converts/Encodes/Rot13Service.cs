using System.Text;

namespace nHash.Application.Encodes;

public class Rot13Service : IRot13Service
{
    public string Calculate(string text, int shift)
    {
        // Normalize shift to 0-25 range
        shift = ((shift % 26) + 26) % 26;

        var result = new StringBuilder(text.Length);
        foreach (var ch in text)
        {
            if (char.IsAsciiLetter(ch))
            {
                var baseChar = char.IsUpper(ch) ? 'A' : 'a';
                result.Append((char)(((ch - baseChar + shift) % 26) + baseChar));
            }
            else
            {
                result.Append(ch);
            }
        }

        return result.ToString();
    }
}
