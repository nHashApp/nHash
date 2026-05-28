using SHA3.Net;

namespace nHash.Application.Hashes.Algorithms;

public class Sha3224Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => Sha3.Sha3224().ComputeHash(buffer);

}