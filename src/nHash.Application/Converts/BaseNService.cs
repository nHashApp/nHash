using System.Numerics;
using System.Text;

namespace nHash.Application.Converts;

public class BaseNService : IBaseNService
{
    private const string Digits = "0123456789abcdefghijklmnopqrstuvwxyz";

    public string Convert(string number, int fromBase, int toBase)
    {
        if (string.IsNullOrWhiteSpace(number))
            return string.Empty;

        if (fromBase < 2 || fromBase > 36 || toBase < 2 || toBase > 36)
            return "Error: Base must be between 2 and 36.";

        try
        {
            var value = Parse(number.Trim().ToLowerInvariant(), fromBase);
            return Format(value, toBase);
        }
        catch (Exception ex)
        {
            return $"Error during conversion: {ex.Message}";
        }
    }

    private static BigInteger Parse(string number, int fromBase)
    {
        BigInteger result = 0;
        BigInteger baseValue = fromBase;
        
        bool isNegative = false;
        if (number.StartsWith('-'))
        {
            isNegative = true;
            number = number[1..];
        }

        foreach (char c in number)
        {
            int digitValue = Digits.IndexOf(c);
            if (digitValue == -1 || digitValue >= fromBase)
            {
                throw new ArgumentException($"Invalid character '{c}' for base {fromBase}.");
            }
            result = result * baseValue + digitValue;
        }

        return isNegative ? -result : result;
    }

    private static string Format(BigInteger value, int toBase)
    {
        if (value == 0) return "0";

        bool isNegative = value < 0;
        if (isNegative) value = -value;

        var sb = new StringBuilder();
        BigInteger baseValue = toBase;

        while (value > 0)
        {
            var rem = (int)(value % baseValue);
            sb.Append(Digits[rem]);
            value /= baseValue;
        }

        if (isNegative) sb.Append('-');

        char[] arr = sb.ToString().ToCharArray();
        Array.Reverse(arr);
        return new string(arr);
    }
}
