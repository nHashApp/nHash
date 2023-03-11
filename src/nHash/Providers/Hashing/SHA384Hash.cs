using System.Security.Cryptography;

namespace nHash.Providers.Hashing;

public class SHA384Hash : IHash
{
    private readonly SHA384 _provider = SHA384.Create();

    public byte[] ComputeHash(byte[] buffer)
    {
        return _provider.ComputeHash(buffer);
    }
}