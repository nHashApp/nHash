using System.IO.Hashing;

namespace nHash.Application.Cryptos.Hashes.Algorithms;

public class XxHash128Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
    {
        return XxHash3.Hash(buffer);
    }
}
