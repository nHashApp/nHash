using System.Security.Cryptography;

namespace nHash.Features.HashAlgorithms;

public class Sha512Feature : BaseHashAlgorithm
{
    public Sha512Feature() : base("sha512", "SHA512")
    {
    }

    protected override byte[] CalculateHash(byte[] input)
        => SHA512.HashData(input);
}