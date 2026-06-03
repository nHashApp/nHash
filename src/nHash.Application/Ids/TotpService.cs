using System.Security.Cryptography;
using nHash.Application.Ids.Models;

namespace nHash.Application.Ids;

public class TotpService : ITotpService
{
    public TotpGenerateResult Generate(string secretBase32, int digits, int periodSeconds)
    {
        var result = new TotpGenerateResult();
        if (string.IsNullOrWhiteSpace(secretBase32))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Secret cannot be empty.";
            return result;
        }

        if (digits < 4 || digits > 10)
        {
            result.Success = false;
            result.ErrorMessage = "Error: Digits must be between 4 and 10.";
            return result;
        }

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

            result.Code = code.ToString().PadLeft(digits, '0');
            result.Digits = digits;
            result.PeriodSeconds = periodSeconds;
            result.RemainingSeconds = remaining;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error generating TOTP: {ex.Message}";
            return result;
        }
    }

    public TotpRemainingResult Remaining(int periodSeconds)
    {
        int remaining = periodSeconds - (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() % periodSeconds);
        return new TotpRemainingResult
        {
            RemainingSeconds = remaining,
            PeriodSeconds = periodSeconds
        };
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
