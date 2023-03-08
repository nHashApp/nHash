using System.Security.Cryptography;

namespace nHash.Features.HashAlgorithms;

public class Sha256Feature : BaseHashAlgorithm
{
    public Sha256Feature() : base("sha256", "SHA256")
    {
    }

    protected override byte[] CalculateHash(byte[] input)
        => SHA256.HashData(input);
}