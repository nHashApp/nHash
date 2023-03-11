using System.Security.Cryptography;

namespace nHash.Providers.Hashing;

public class MD5Hash : IHash
{
    private readonly MD5 _provider = MD5.Create();

    public byte[] ComputeHash(byte[] buffer)
    {
        return _provider.ComputeHash(buffer);
    }
}