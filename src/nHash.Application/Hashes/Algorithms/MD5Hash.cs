using System.Security.Cryptography;

namespace nHash.Application.Hashes.Algorithms;

internal class MD5Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => MD5.HashData(buffer);
    
}