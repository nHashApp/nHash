using System.Numerics;
using System.Text;

namespace nHash.Application.Encodes;

public class Base58Service : IBase58Service
{
    private const string Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

    public string Calculate(string text, bool decode)
    {
        var resultText = !decode
            ? Encode(text)
            : Decode(text);

        return resultText;
    }

    private static string Encode(string text)
    {
        var input = Encoding.UTF8.GetBytes(text);
        
        // Count leading zeros
        int zeros = 0;
        while (zeros < input.Length && input[zeros] == 0)
        {
            ++zeros;
        }

        // Convert bytes to BigInteger
        var num = new BigInteger(input.Reverse().ToArray());

        // Build the string
        var sb = new StringBuilder();
        while (num > 0)
        {
            num = BigInteger.DivRem(num, 58, out BigInteger remainder);
            sb.Append(Alphabet[(int)remainder]);
        }

        // Add leading zeros
        sb.Append(Alphabet[0], zeros);

        return new string(sb.ToString().Reverse().ToArray());
    }

    private static string Decode(string input)
    {
        // Convert the string to BigInteger
        var num = new BigInteger(0);
        foreach (var c in input)
        {
            var value = Alphabet.IndexOf(c);
            if (value < 0)
            {
                throw new ArgumentException("Invalid character in Base58 string.");
            }
            num = num * 58 + value;
        }

        // Convert BigInteger to byte array
        var bytes = num.ToByteArray().Reverse().ToArray();

        // Remove leading zeros
        var zeros = 0;
        while (zeros < bytes.Length && bytes[zeros] == 0)
        {
            ++zeros;
        }

        // Build the result string
        var sb = new StringBuilder();
        sb.Append('\0', zeros);
        sb.Append(Encoding.UTF8.GetString(bytes, zeros, bytes.Length - zeros));
        return sb.ToString();
    }
}