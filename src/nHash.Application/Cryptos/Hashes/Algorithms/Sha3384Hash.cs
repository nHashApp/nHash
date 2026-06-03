using SHA3.Net;

namespace nHash.Application.Cryptos.Hashes.Algorithms;

public class Sha3384Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => Sha3.Sha3384().ComputeHash(buffer);

}