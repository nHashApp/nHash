using System.Security.Cryptography;
using System.Text;

namespace nHash.Application.Ids;

public class CuidService : ICuidService
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
    private static readonly object Lock = new();
    private static uint _counter = 0;

    static CuidService()
    {
        var bytes = new byte[4];
        RandomNumberGenerator.Fill(bytes);
        _counter = BitConverter.ToUInt32(bytes, 0);
    }

    public string Generate(int length = 24)
    {
        if (length < 4) length = 24;

        var sb = new StringBuilder();
        
        var firstByte = new byte[1];
        RandomNumberGenerator.Fill(firstByte);
        sb.Append(Alphabet[firstByte[0] % 26]);

        var ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        sb.Append(ToBase36(ms));

        uint currentCounter;
        lock (Lock)
        {
            currentCounter = _counter++;
        }
        sb.Append(ToBase36(currentCounter));

        int remaining = length - sb.Length;
        if (remaining > 0)
        {
            var randBytes = new byte[remaining];
            RandomNumberGenerator.Fill(randBytes);
            for (int i = 0; i < remaining; i++)
            {
                sb.Append(Alphabet[randBytes[i] % Alphabet.Length]);
            }
        }

        return sb.ToString().Substring(0, length);
    }

    private static string ToBase36(long value)
    {
        const string Base36Alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";
        var sb = new StringBuilder();
        while (value > 0)
        {
            sb.Append(Base36Alphabet[(int)(value % 36)]);
            value /= 36;
        }
        var chars = sb.ToString().ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}
