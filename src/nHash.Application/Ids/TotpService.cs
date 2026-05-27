using System.Security.Cryptography;
using System.Text;

namespace nHash.Application.Ids;

public class TotpService : ITotpService
{
    public string Generate(string secretBase32, int digits, int periodSeconds)
    {
        if (string.IsNullOrWhiteSpace(secretBase32))
            return "Error: Secret cannot be empty.";

        try
        {
            var key = Base32Decode(secretBase32);
            var counter = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / periodSeconds;

            var counterBytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(counterBytes);

            using var hmac = new HMACSHA1(key);
            var hash = hmac.ComputeHash(counterBytes);

            int offset = hash[19] & 0x0F;
            int code = ((hash[offset] & 0x7F) << 24)
                     | (hash[offset + 1] << 16)
                     | (hash[offset + 2] << 8)
                     | hash[offset + 3];

            int divisor = (int)Math.Pow(10, digits);
            code = code % divisor;

            int remaining = periodSeconds - (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() % periodSeconds);

            var sb = new StringBuilder();
            sb.AppendLine($"OTP Code:          {code.ToString().PadLeft(digits, '0')}");
            sb.AppendLine($"Digits:            {digits}");
            sb.AppendLine($"Period:            {periodSeconds}s");
            sb.AppendLine($"Remaining:         {remaining}s");
            sb.AppendLine($"Algorithm:         HMAC-SHA1 (RFC 6238)");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Error generating TOTP: {ex.Message}";
        }
    }

    public string Remaining(int periodSeconds)
    {
        int remaining = periodSeconds - (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() % periodSeconds);
        return $"Remaining: {remaining}s of {periodSeconds}s period";
    }

    private static byte[] Base32Decode(string base32)
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        base32 = base32.ToUpperInvariant().TrimEnd('=');
        var bits = 0;
        var accumulator = 0;
        var output = new List<byte>();
        foreach (var c in base32)
        {
            var value = alphabet.IndexOf(c);
            if (value < 0) continue;
            accumulator = (accumulator << 5) | value;
            bits += 5;
            if (bits >= 8) { bits -= 8; output.Add((byte)(accumulator >> bits)); }
        }
        return [.. output];
    }
}
