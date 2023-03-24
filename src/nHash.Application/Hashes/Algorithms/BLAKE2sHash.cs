using Blake2Fast;

namespace nHash.Application.Hashes.Algorithms;

internal class BLAKE2sHash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => Blake2s.ComputeHash(buffer);
}