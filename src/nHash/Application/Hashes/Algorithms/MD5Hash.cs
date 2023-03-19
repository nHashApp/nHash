using System.Security.Cryptography;

namespace nHash.Application.Hashes.Algorithms;

public class MD5Hash : IHash
{
    private readonly MD5 _provider = MD5.Create();

    public byte[] ComputeHash(byte[] buffer)
    {
        return _provider.ComputeHash(buffer);
    }
}