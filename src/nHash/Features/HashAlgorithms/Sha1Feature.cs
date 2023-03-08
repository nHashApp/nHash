using System.Security.Cryptography;

namespace nHash.Features.HashAlgorithms;

public class Sha1Feature : BaseHashAlgorithm
{
    public Sha1Feature() : base("sha1", "SHA1")
    {
    }

    protected override byte[] CalculateHash(byte[] input)
        => SHA1.HashData(input);
}