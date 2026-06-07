using System.Security.Cryptography;

namespace nHash.Application.Ids;

public class UlidService : IUlidService
{
    private const string Alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

    public UlidResult Generate(int count)
    {
        var result = new UlidResult();
        if (count <= 0)
        {
            result.Success = false;
            result.ErrorMessage = "Count must be greater than 0.";
            return result;
        }

        try
        {
            for (int i = 0; i < count; i++)
            {
                result.Ulids.Add(GenerateSingleUlid());
            }
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"ULID Generation Error: {ex.Message}";
            return result;
        }
    }

    private static string GenerateSingleUlid()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var randomBytes = new byte[10];
        RandomNumberGenerator.Fill(randomBytes);

        var tsPart = EncodeTimestamp(timestamp);
        var randPart = EncodeRandomness(randomBytes);

        return tsPart + randPart;
    }

    private static string EncodeTimestamp(long timestamp)
    {
        char[] chars = new char[10];
        var temp = timestamp;
        for (int i = 9; i >= 0; i--)
        {
            chars[i] = Alphabet[(int)(temp % 32)];
            temp /= 32;
        }
        return new string(chars);
    }

    private static string EncodeRandomness(byte[] bytes)
    {
        char[] chars = new char[16];

        chars[0] = Alphabet[bytes[0] >> 3];
        chars[1] = Alphabet[((bytes[0] & 0x07) << 2) | (bytes[1] >> 6)];
        chars[2] = Alphabet[(bytes[1] & 0x3E) >> 1];
        chars[3] = Alphabet[((bytes[1] & 0x01) << 4) | (bytes[2] >> 4)];
        chars[4] = Alphabet[((bytes[2] & 0x0F) << 1) | (bytes[3] >> 7)];
        chars[5] = Alphabet[(bytes[3] & 0x7C) >> 2];
        chars[6] = Alphabet[((bytes[3] & 0x03) << 3) | (bytes[4] >> 5)];
        chars[7] = Alphabet[bytes[4] & 0x1F];
        chars[8] = Alphabet[bytes[5] >> 3];
        chars[9] = Alphabet[((bytes[5] & 0x07) << 2) | (bytes[6] >> 6)];
        chars[10] = Alphabet[(bytes[6] & 0x3E) >> 1];
        chars[11] = Alphabet[((bytes[6] & 0x01) << 4) | (bytes[7] >> 4)];
        chars[12] = Alphabet[((bytes[7] & 0x0F) << 1) | (bytes[8] >> 7)];
        chars[13] = Alphabet[(bytes[8] & 0x7C) >> 2];
        chars[14] = Alphabet[((bytes[8] & 0x03) << 3) | (bytes[9] >> 5)];
        chars[15] = Alphabet[bytes[9] & 0x1F];

        return new string(chars);
    }
}
