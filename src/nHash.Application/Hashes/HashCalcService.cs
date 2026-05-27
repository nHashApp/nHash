using nHash.Application.Hashes.Algorithms;
using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public class HashCalcService : IHashCalcService
{
    public Dictionary<HashType, string> CalculateText(byte[] inputBytes, bool lowerCase, HashType hashType)
    {
        return CalculateHash(inputBytes, lowerCase, hashType);
    }

    private static Dictionary<HashType, string> CalculateHash(byte[] inputBytes, bool lowerCase, HashType hashType)
    {
        var result = new Dictionary<HashType, string>();
        if (hashType != HashType.All)
        {
            var returnedHash = CalculateHashText(inputBytes, lowerCase, hashType);
            result.Add(hashType, returnedHash);
            return result;
        }

        foreach (var algorithm in Enum.GetValues<HashType>())
        {
            if (algorithm == HashType.All)
            {
                continue;
            }

            var returnedHash = CalculateHashText(inputBytes, lowerCase, algorithm);
            result.Add(algorithm, returnedHash);
        }

        return result;
    }

    private static string CalculateHashText(byte[] inputBytes, bool lowerCase, HashType hashType)
    {
        var hashedText = CalculateHashType(inputBytes, hashType);

        if (lowerCase)
        {
            hashedText = hashedText.ToLower();
        }

        return hashedText;
    }

    private static string CalculateHashType(byte[] inputBytes, HashType hashType)
    {
        IHash provider = hashType switch
        {
            HashType.Md5 => new Md5Hash(),
            HashType.Sha1 => new Sha1Hash(),
            HashType.Sha256 => new Sha256Hash(),
            HashType.Sha384 => new Sha384Hash(),
            HashType.Sha512 => new Sha512Hash(),
            HashType.Sha3224 => new Sha3224Hash(),
            HashType.Sha3256 => new Sha3256Hash(),
            HashType.Sha3384 => new Sha3384Hash(),
            HashType.Sha3512 => new Sha3512Hash(),
            HashType.Blake2B => new Blake2BHash(),
            HashType.Blake2S => new Blake2SHash(),
            _ => new Md5Hash()
        };

        var hashBytes = provider.ComputeHash(inputBytes);
        var hashedText = Convert.ToHexString(hashBytes);
        return hashedText;
    }
}