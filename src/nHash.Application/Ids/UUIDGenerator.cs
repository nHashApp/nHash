using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace nHash.Application.Uuids;

public class UuidGenerator : IUuidGenerator
{
    public Guid GenerateUuiDv1()
    {
        var currentTime = DateTime.UtcNow;
        var timeBytes = BitConverter.GetBytes(currentTime.Ticks);
        Array.Reverse(timeBytes);
        var nodeId = GetMachineIdentifier();
        var versionAndVariant = new byte[] { 0b00000001, 0b10000000 };
        var uuidBytes = new byte[16];
        Array.Copy(timeBytes, 2, uuidBytes, 0, 6);
        Array.Copy(nodeId, 0, uuidBytes, 6, 6);
        Array.Copy(versionAndVariant, 0, uuidBytes, 12, 2);
        return new Guid(uuidBytes);
    }

    public Guid GenerateUuiDv2()
    {
        var currentTime = DateTime.UtcNow;
        var timeBytes = BitConverter.GetBytes(currentTime.Ticks);
        Array.Reverse(timeBytes);
        var nodeId = GetMachineIdentifier();
        var versionAndVariant = new byte[] { 0b00000010, 0b10000000 };

        var uuidBytes = new byte[16];
        Array.Copy(timeBytes, 2, uuidBytes, 0, 4);
        Array.Copy(nodeId, 0, uuidBytes, 4, 2);
        Array.Copy(versionAndVariant, 0, uuidBytes, 6, 2);
        return new Guid(uuidBytes);
    }

    public Guid GenerateUuiDv3(Guid namespaceId, string name)
    {
        var nameBytes = System.Text.Encoding.UTF8.GetBytes(name);
        var namespaceBytes = namespaceId.ToByteArray();
        var hashBytes = MD5.HashData(ConcatenateArrays(namespaceBytes, nameBytes));
        var uuidBytes = new byte[16];
        Array.Copy(hashBytes, 0, uuidBytes, 0, 16);
        uuidBytes[6] &= 0x0f;
        uuidBytes[6] |= 3 << 4;
        uuidBytes[8] &= 0x3f;
        uuidBytes[8] |= 0x80;
        return new Guid(uuidBytes);
    }

    public Guid GenerateUuiDv4()
    {
        var uuidBytes = new byte[16];
        RandomNumberGenerator.Create().GetBytes(uuidBytes);
        uuidBytes[6] &= 0x0f;
        uuidBytes[6] |= 4 << 4;
        uuidBytes[8] &= 0x3f;
        uuidBytes[8] |= 0x80;
        return new Guid(uuidBytes);
    }

    public Guid GenerateUuiDv5(Guid namespaceId, string name)
    {
        var nameBytes = System.Text.Encoding.UTF8.GetBytes(name);
        var namespaceBytes = namespaceId.ToByteArray();
        var hashBytes = SHA1.Create().ComputeHash(ConcatenateArrays(namespaceBytes, nameBytes));
        var uuidBytes = new byte[16];
        Array.Copy(hashBytes, 0, uuidBytes, 0, 16);
        uuidBytes[6] &= 0x0f;
        uuidBytes[6] |= 5 << 4;
        uuidBytes[8] &= 0x3f;
        uuidBytes[8] |= 0x80;
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
            Array.Copy(customData.ToArray(), bytes, Math.Min(customData.Length, 16));
        }
        else
        {
            RandomNumberGenerator.Fill(bytes);
        }
        bytes[6] &= 0x0f;
        bytes[6] |= 8 << 4;
        bytes[8] &= 0x3f;
        bytes[8] |= 0x80;
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
        var macAddress = new byte[6];
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        if (interfaces.Length > 0)
        {
            var address = interfaces[0].GetPhysicalAddress();
            macAddress = address.GetAddressBytes();
        }

        macAddress[0] |= 0b00000001;
        macAddress[0] |= 0b00000010;
        return macAddress;
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