using System.Security.Cryptography;

namespace nHash.Features.HashAlgorithms;

public class Md5Feature : BaseHashAlgorithm
{
    public Md5Feature() : base("md5", "MD5")
    {
    }

    protected override byte[] CalculateHash(byte[] input)
        => MD5.HashData(input);
}