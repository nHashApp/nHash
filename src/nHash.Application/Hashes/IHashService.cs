using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public interface IHashService
{
    Dictionary<HashType, string> CalculateText(byte[] inputBytes, bool lowerCase, HashType hashType);
}