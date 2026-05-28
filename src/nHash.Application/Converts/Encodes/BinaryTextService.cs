using System.Text;

namespace nHash.Application.Encodes;

public class BinaryTextService : IBinaryTextService
{
    public string Calculate(string text, bool decode, int numericBase)
    {
        return decode ? Decode(text, numericBase) : Encode(text, numericBase);
    }

    private static string Encode(string text, int numericBase)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        var parts = new string[bytes.Length];

        for (var i = 0; i < bytes.Length; i++)
        {
            parts[i] = numericBase switch
            {
                2 => Convert.ToString(bytes[i], 2).PadLeft(8, '0'),
                8 => Convert.ToString(bytes[i], 8).PadLeft(3, '0'),
                10 => bytes[i].ToString(),
                _ => throw new ArgumentException($"Unsupported base: {numericBase}. Supported bases are 2, 8, and 10.")
            };
        }

        return string.Join(" ", parts);
    }

    private static string Decode(string encodedText, int numericBase)
    {
        var parts = encodedText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var bytes = new byte[parts.Length];

        for (var i = 0; i < parts.Length; i++)
        {
            bytes[i] = numericBase switch
            {
                2 => Convert.ToByte(parts[i], 2),
                8 => Convert.ToByte(parts[i], 8),
                10 => byte.Parse(parts[i]),
                _ => throw new ArgumentException($"Unsupported base: {numericBase}. Supported bases are 2, 8, and 10.")
            };
        }

        return Encoding.UTF8.GetString(bytes);
    }
}
