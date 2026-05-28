using Blake2Fast;

namespace nHash.Application.Hashes.Algorithms;

internal class Blake2SHash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => Blake2s.ComputeHash(buffer);
}