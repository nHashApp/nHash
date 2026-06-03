using System.Security.Cryptography;

namespace nHash.Application.Cryptos.Hashes.Algorithms;

internal class Sha256Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => SHA256.HashData(buffer);
}