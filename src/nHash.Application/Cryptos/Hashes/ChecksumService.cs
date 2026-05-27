using nHash.Application.Hashes.Algorithms;
using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public class ChecksumService : IChecksumService
{
    public Dictionary<ChecksumType, string> CalculateText(byte[] inputBytes, bool lowerCase, ChecksumType hashType)
    {
        return CalculateHash(inputBytes, lowerCase, hashType);
    }

    public (string NewChecksum, bool IsMatch) VerifyChecksum(byte[] inputBytes, string checksum, ChecksumType hashType)
    {
        var newChecksumList = CalculateHash(inputBytes, false, hashType);

        var newChecksum = newChecksumList.First().Value;
        var targetChecksum = checksum.Replace("-", "").Replace(".", "");
        var isMatch = string.Equals(newChecksum, targetChecksum, StringComparison.CurrentCultureIgnoreCase);
        return (newChecksum, isMatch);
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
            ChecksumType.Md5 => new Md5Hash(),
            ChecksumType.Sha1 => new Sha1Hash(),
            ChecksumType.Crc8 => new Crc8Hash(),
            ChecksumType.Crc32 => new Crc32Hash(),
            ChecksumType.Adler32 => new Adler32Hash(),
            ChecksumType.Fletcher16 => new Fletcher16Hash(),
            ChecksumType.Fletcher32 => new Fletcher32Hash(),
            ChecksumType.Crc16 => new Crc16Hash(),
            _ => new Md5Hash()
        };

        var hashBytes = provider.ComputeHash(inputBytes);
        var hashedText = Convert.ToHexString(hashBytes);
        return hashedText;
    }
}