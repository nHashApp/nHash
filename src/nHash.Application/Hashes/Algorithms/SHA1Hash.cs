using System.Security.Cryptography;

namespace nHash.Application.Hashes.Algorithms;

internal class Sha1Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => SHA1.HashData(buffer);
}