using System.IO.Hashing;

namespace nHash.Application.Hashes.Algorithms;

public class XxHash128Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
    {
        return XxHash3.Hash(buffer);
    }
}
