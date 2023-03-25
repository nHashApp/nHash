using System.Security.Cryptography;

namespace nHash.Application.Hashes.Algorithms;

internal class SHA512Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => SHA512.HashData(buffer);
    
}