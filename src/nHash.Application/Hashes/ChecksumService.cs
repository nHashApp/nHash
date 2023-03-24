using nHash.Application.Hashes.Algorithms;
using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public class ChecksumService : IChecksumService
{
    public Dictionary<ChecksumType, string> CalculateText(byte[] inputBytes, bool lowerCase, ChecksumType hashType)
    {
        return CalculateHash(inputBytes, lowerCase, hashType);
    }

    private static Dictionary<ChecksumType, string> CalculateHash(byte[] inputBytes, bool lowerCase,
        ChecksumType hashType)
    {
        var result = new Dictionary<ChecksumType, string>();
        if (hashType != ChecksumType.All)
        {
            var returnedHash = CalculateHashText(inputBytes, lowerCase, hashType);
            result.Add(hashType, returnedHash);
            return result;
        }

        foreach (var algorithm in Enum.GetValues<ChecksumType>())
        {
            if (algorithm == ChecksumType.All)
            {
                continue;
            }

            var returnedHash = CalculateHashText(inputBytes, lowerCase, algorithm);
            result.Add(algorithm, returnedHash);
        }

        return result;
    }

    private static string CalculateHashText(byte[] inputBytes, bool lowerCase, ChecksumType hashType)
    {
        var hashedText = CalculateHashType(inputBytes, hashType);

        if (lowerCase)
        {
            hashedText = hashedText.ToLower();
        }

        return hashedText;
    }

    private static string CalculateHashType(byte[] inputBytes, ChecksumType hashType)
    {
        IHash provider = hashType switch
        {
            ChecksumType.MD5 => new MD5Hash(),
            ChecksumType.SHA1 => new SHA1Hash(),
            ChecksumType.CRC8 => new CRC8Hash(),
            ChecksumType.CRC32 => new CRC32Hash(),
            ChecksumType.Adler32 => new Adler32Hash(),
            ChecksumType.Fletcher16 => new Fletcher16Hash(),
            ChecksumType.Fletcher32 => new Fletcher32Hash(),
            _ => new MD5Hash()
        };

        var hashBytes = provider.ComputeHash(inputBytes);
        var hashedText = Convert.ToHexString(hashBytes);
        return hashedText;
    }
}