using Blake2Fast;

namespace nHash.Application.Cryptos.Hashes.Algorithms;

internal class Blake2BHash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => Blake2b.ComputeHash(buffer);
}