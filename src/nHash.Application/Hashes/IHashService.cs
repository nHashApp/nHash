using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public interface IHashService
{
    Task CalculateText(string text, bool lowerCase, string fileName, HashType hashType);
}