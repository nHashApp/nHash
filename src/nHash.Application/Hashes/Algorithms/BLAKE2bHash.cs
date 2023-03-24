using Blake2Fast;

namespace nHash.Application.Hashes.Algorithms;

internal class BLAKE2bHash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => Blake2b.ComputeHash(buffer);
}