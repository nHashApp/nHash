using System.Security.Cryptography;

namespace nHash.Features.HashAlgorithms;

public class Sha384Feature : BaseHashAlgorithm
{
    public Sha384Feature() : base("sha384", "SHA384")
    {
    }

    protected override byte[] CalculateHash(byte[] input)
        => SHA384.HashData(input);
}