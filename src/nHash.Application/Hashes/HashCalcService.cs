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
            HashType.MD5 => new MD5Hash(),
            HashType.SHA1 => new SHA1Hash(),
            HashType.SHA256 => new SHA256Hash(),
            HashType.SHA384 => new SHA384Hash(),
            HashType.SHA512 => new SHA512Hash(),
            HashType.SHA3_224 => new Sha3224Hash(),
            HashType.SHA3_256 => new Sha3256Hash(),
            HashType.SHA3_384 => new Sha3384Hash(),
            HashType.SHA3_512 => new Sha3512Hash(),
            HashType.BLAKE2b => new BLAKE2bHash(),
            HashType.BLAKE2s => new BLAKE2sHash(),
            _ => new MD5Hash()
        };

        var hashBytes = provider.ComputeHash(inputBytes);
        var hashedText = Convert.ToHexString(hashBytes);
        return hashedText;
    }
}