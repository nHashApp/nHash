using System.IO.Hashing;

namespace nHash.Application.Hashes.Algorithms;

public class XxHash64Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
    {
        return XxHash64.Hash(buffer);
    }
}
