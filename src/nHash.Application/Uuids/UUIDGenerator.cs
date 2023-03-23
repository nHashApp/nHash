using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace nHash.Application.Uuids;

public class UUIDGenerator : IUUIDGenerator
{
    public Guid GenerateUUIDv1()
    {
        var currentTime = DateTime.UtcNow;
        var timeBytes = BitConverter.GetBytes(currentTime.Ticks);
        Array.Reverse(timeBytes);
        var nodeId = GetMachineIdentifier();
        var versionAndVariant = new byte[2] { (byte)0b00000001, (byte)0b10000000 };
        var uuidBytes = new byte[16];
        Array.Copy(timeBytes, 2, uuidBytes, 0, 6);
        Array.Copy(nodeId, 0, uuidBytes, 6, 6);
        Array.Copy(versionAndVariant, 0, uuidBytes, 12, 2);
        return new Guid(uuidBytes);
    }

    public Guid GenerateUUIDv2()
    {
        var currentTime = DateTime.UtcNow;
        var timeBytes = BitConverter.GetBytes(currentTime.Ticks);
        Array.Reverse(timeBytes);
        var nodeId = GetMachineIdentifier();
        var versionAndVariant = new byte[2] { (byte)0b00000010, (byte)0b10000000 };

        var uuidBytes = new byte[16];
        Array.Copy(timeBytes, 2, uuidBytes, 0, 4);
        Array.Copy(nodeId, 0, uuidBytes, 4, 2);
        Array.Copy(versionAndVariant, 0, uuidBytes, 6, 2);
        return new Guid(uuidBytes);
    }

    public Guid GenerateUUIDv3(Guid namespaceId, string name)
    {
        var nameBytes = System.Text.Encoding.UTF8.GetBytes(name);
        var namespaceBytes = namespaceId.ToByteArray();
        var hashBytes = MD5.HashData(ConcatenateArrays(namespaceBytes, nameBytes));
        var uuidBytes = new byte[16];
        Array.Copy(hashBytes, 0, uuidBytes, 0, 16);
        uuidBytes[6] &= 0x0f;
        uuidBytes[6] |= (byte)(3 << 4);
        uuidBytes[8] &= 0x3f;
        uuidBytes[8] |= 0x80;
        return new Guid(uuidBytes);
    }

    public Guid GenerateUUIDv4()
    {
        var uuidBytes = new byte[16];
        RandomNumberGenerator.Create().GetBytes(uuidBytes);
        uuidBytes[6] &= 0x0f;
        uuidBytes[6] |= (byte)(4 << 4);
        uuidBytes[8] &= 0x3f;
        uuidBytes[8] |= 0x80;
        return new Guid(uuidBytes);
    }

    public Guid GenerateUUIDv5(Guid namespaceId, string name)
    {
        var nameBytes = System.Text.Encoding.UTF8.GetBytes(name);
        var namespaceBytes = namespaceId.ToByteArray();
        var hashBytes = SHA1.Create().ComputeHash(ConcatenateArrays(namespaceBytes, nameBytes));
        var uuidBytes = new byte[16];
        Array.Copy(hashBytes, 0, uuidBytes, 0, 16);
        uuidBytes[6] &= 0x0f;
        uuidBytes[6] |= (byte)(5 << 4);
        uuidBytes[8] &= 0x3f;
        uuidBytes[8] |= 0x80;
        return new Guid(uuidBytes);
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