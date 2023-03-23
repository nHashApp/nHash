using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public interface IHashService
{
    void CalculateText(byte[] inputBytes, bool lowerCase, HashType hashType);
}