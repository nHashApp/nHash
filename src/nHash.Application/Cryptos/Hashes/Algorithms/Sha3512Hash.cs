using SHA3.Net;

namespace nHash.Application.Hashes.Algorithms;

public class Sha3512Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => Sha3.Sha3512().ComputeHash(buffer);

}