using SHA3.Net;

namespace nHash.Application.Cryptos.Hashes.Algorithms;

public class Sha3256Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
        => Sha3.Sha3256().ComputeHash(buffer);

}