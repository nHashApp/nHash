using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public interface IHashCalcService
{
    Dictionary<HashType, string> CalculateText(byte[] inputBytes, bool lowerCase, HashType hashType);
}