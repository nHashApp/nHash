using System.Security.Cryptography;

namespace nHash.Application.Providers.Hashing;

public class SHA1Hash : IHash
{
    private readonly SHA1 _provider = SHA1.Create();

    public byte[] ComputeHash(byte[] buffer)
    {
        return _provider.ComputeHash(buffer);
    }
}