using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public interface IChecksumService
{
    Dictionary<ChecksumType, string> CalculateText(byte[] inputBytes, bool lowerCase, ChecksumType hashType);
    (string NewChecksum, bool IsMatch) VerifyChecksum(byte[] inputBytes, string checksum, ChecksumType hashType);
}