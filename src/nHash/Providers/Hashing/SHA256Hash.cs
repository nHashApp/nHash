using System.Security.Cryptography;

namespace nHash.Providers.Hashing;

public class SHA256Hash : IHash
{
    private readonly SHA256 _provider = SHA256.Create();

    public byte[] ComputeHash(byte[] buffer)
    {
        return _provider.ComputeHash(buffer);
    }
}