using nHash.Application.Cryptos.Hashes.Models;

namespace nHash.Application.Cryptos.Hashes;

public interface IHashCalcService
{
    Dictionary<HashType, string> CalculateText(byte[] inputBytes, bool lowerCase, HashType hashType);
}