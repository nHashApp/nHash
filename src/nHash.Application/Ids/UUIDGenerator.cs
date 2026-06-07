using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace nHash.Application.Ids;

public class UuidGenerator : IUuidGenerator
{
    public Guid GenerateUuiDv1()
    {
        var gregorianEpoch = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);
        var ticks = DateTime.UtcNow.Ticks - gregorianEpoch.Ticks;

        var timeLow = (int)(ticks & 0xFFFFFFFF);
        var timeMid = (short)((ticks >> 32) & 0xFFFF);
        var timeHi = (short)(((ticks >> 48) & 0x0FFF) | (1 << 12)); // Version 1

        var clockSeq = (short)(RandomNumberGenerator.GetInt32(0, 16384) & 0x3FFF);
        var clockSeqHi = (byte)(((clockSeq >> 8) & 0x3F) | 0x80);
        var clockSeqLow = (byte)(clockSeq & 0xFF);

        var nodeId = GetMachineIdentifier();

        return new Guid(timeLow, timeMid, timeHi, clockSeqHi, clockSeqLow, nodeId[0], nodeId[1], nodeId[2], nodeId[3], nodeId[4], nodeId[5]);
    }

    public Guid GenerateUuiDv2()
    {
        var gregorianEpoch = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);
        var ticks = DateTime.UtcNow.Ticks - gregorianEpoch.Ticks;

        var timeLow = (int)(ticks & 0xFFFFFFFF);
        var timeMid = (short)((ticks >> 32) & 0xFFFF);
        var timeHi = (short)(((ticks >> 48) & 0x0FFF) | (2 << 12)); // Version 2

        var clockSeq = (short)(RandomNumberGenerator.GetInt32(0, 16384) & 0x3FFF);
        var clockSeqHi = (byte)(((clockSeq >> 8) & 0x3F) | 0x80);
        var clockSeqLow = (byte)(clockSeq & 0xFF);

        var nodeId = GetMachineIdentifier();

        return new Guid(timeLow, timeMid, timeHi, clockSeqHi, clockSeqLow, nodeId[0], nodeId[1], nodeId[2], nodeId[3], nodeId[4], nodeId[5]);
    }

    public Guid GenerateUuiDv3(Guid namespaceId, string name)
    {
        var nameBytes = System.Text.Encoding.UTF8.GetBytes(name);
        var namespaceBytes = namespaceId.ToByteArray();
        var hashBytes = MD5.HashData(ConcatenateArrays(namespaceBytes, nameBytes));
        var uuidBytes = new byte[16];
        Array.Copy(hashBytes, 0, uuidBytes, 0, 16);

        if (BitConverter.IsLittleEndian)
        {
            uuidBytes[7] &= 0x0F;
            uuidBytes[7] |= 3 << 4; // Version 3
        }
        else
        {
            uuidBytes[6] &= 0x0F;
            uuidBytes[6] |= 3 << 4;
        }

        uuidBytes[8] &= 0x3F;
        uuidBytes[8] |= 0x80; // Variant

        return new Guid(uuidBytes);
    }

    public Guid GenerateUuiDv4()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);

        if (BitConverter.IsLittleEndian)
        {
            bytes[7] &= 0x0F;
            bytes[7] |= 4 << 4; // Version 4
        }
        else
        {
            bytes[6] &= 0x0F;
            bytes[6] |= 4 << 4;
        }

        bytes[8] &= 0x3F;
        bytes[8] |= 0x80; // Variant

        return new Guid(bytes);
    }

    public Guid GenerateUuiDv5(Guid namespaceId, string name)
    {
        var nameBytes = System.Text.Encoding.UTF8.GetBytes(name);
        var namespaceBytes = namespaceId.ToByteArray();
        var hashBytes = SHA1.HashData(ConcatenateArrays(namespaceBytes, nameBytes));
        var uuidBytes = new byte[16];
        Array.Copy(hashBytes, 0, uuidBytes, 0, 16);

        if (BitConverter.IsLittleEndian)
        {
            uuidBytes[7] &= 0x0F;
            uuidBytes[7] |= 5 << 4; // Version 5
        }
        else
        {
            uuidBytes[6] &= 0x0F;
            uuidBytes[6] |= 5 << 4;
        }

        uuidBytes[8] &= 0x3F;
        uuidBytes[8] |= 0x80; // Variant

        return new Guid(uuidBytes);
    }

    public Guid GenerateUuiDv7()
    {
        return Guid.CreateVersion7();
    }

    public Guid GenerateUuiDv8(ReadOnlySpan<byte> customData)
    {
        var bytes = new byte[16];
        if (customData.Length > 0)
        {
            Array.Copy(customData.ToArray(), bytes, System.Math.Min(customData.Length, 16));
        }
        else
        {
            RandomNumberGenerator.Fill(bytes);
        }

        if (BitConverter.IsLittleEndian)
        {
            bytes[7] &= 0x0F;
            bytes[7] |= 8 << 4; // Version 8
        }
        else
        {
            bytes[6] &= 0x0F;
            bytes[6] |= 8 << 4;
        }

        bytes[8] &= 0x3F;
        bytes[8] |= 0x80; // Variant

        return new Guid(bytes);
    }

    public string GenerateUlid()
    {
        const string CrockfordBase32 = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        Span<char> chars = stackalloc char[26];
        
        // Encode Timestamp (10 chars)
        var t = timestamp;
        for (int i = 9; i >= 0; i--)
        {
            chars[i] = CrockfordBase32[(int)(t % 32)];
            t /= 32;
        }
        
        // Encode Randomness (16 chars)
        Span<byte> randomValues = stackalloc byte[16];
        RandomNumberGenerator.Fill(randomValues);
        for (int i = 0; i < 16; i++)
        {
            chars[10 + i] = CrockfordBase32[randomValues[i] % 32];
        }
        
        return new string(chars);
    }

    public string GenerateNanoId(int size = 21)
    {
        const string Alphabet = "_-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        Span<byte> bytes = stackalloc byte[size];
        RandomNumberGenerator.Fill(bytes);
        Span<char> chars = stackalloc char[size];
        for (int i = 0; i < size; i++)
        {
            chars[i] = Alphabet[bytes[i] % Alphabet.Length];
        }
        return new string(chars);
    }

    #region private methods

    private static byte[] GetMachineIdentifier()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var ni in interfaces)
        {
            var address = ni.GetPhysicalAddress();
            if (address != null)
            {
                var bytes = address.GetAddressBytes();
                if (bytes != null && bytes.Length == 6)
                {
                    bytes[0] |= 0b00000001;
                    bytes[0] |= 0b00000010;
                    return bytes;
                }
            }
        }

        var fallback = new byte[6];
        RandomNumberGenerator.Fill(fallback);
        fallback[0] |= 0b00000001;
        fallback[0] |= 0b00000010;
        return fallback;
    }

    private static T[] ConcatenateArrays<T>(T[] a, T[] b)
    {
        var result = new T[a.Length + b.Length];
        Array.Copy(a, result, a.Length);
        Array.Copy(b, 0, result, a.Length, b.Length);
        return result;
    }

    #endregion
}