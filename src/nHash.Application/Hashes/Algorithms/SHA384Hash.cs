using System.Security.Cryptography;

namespace nHash.Application.Hashes.Algorithms;

internal class SHA384Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => SHA384.HashData(buffer);
}