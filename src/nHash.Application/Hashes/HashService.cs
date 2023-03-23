using nHash.Application.Abstraction;
using nHash.Application.Hashes.Algorithms;
using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public class HashService : IHashService
{
    private readonly IFileProvider _fileProvider;
    private readonly IOutputProvider _outputProvider;

    private static readonly Dictionary<HashType, string> Algorithms = new()
    {
        { HashType.MD5, "MD5" },
        { HashType.SHA1, "SHA-1" },
        { HashType.SHA256, "SHA-256" },
        { HashType.SHA384, "SHA-384" },
        { HashType.SHA512, "SHA-512" },
        { HashType.CRC8, "CRC-8" },
        { HashType.CRC32, "CRC-32" },
    };

    public HashService(IFileProvider fileProvider, IOutputProvider outputProvider)
    {
        _fileProvider = fileProvider;
        _outputProvider = outputProvider;
    }

    public async Task CalculateText(string text, bool lowerCase, string fileName, HashType hashType)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
            CalculateHash(inputBytes, lowerCase, hashType);
            return;
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var fileBytes = await _fileProvider.ReadAsByte(fileName);
            if (fileBytes == Array.Empty<byte>())
            {
                return;
            }

            CalculateHash(fileBytes, lowerCase, hashType);
        }
    }

    private void CalculateHash(byte[] inputBytes, bool lowerCase, HashType hashType)
    {
        if (hashType != HashType.All)
        {
            CalculateHashText(inputBytes, lowerCase, hashType);
            return;
        }

        foreach (var algorithm in Algorithms)
        {
            _outputProvider.AppendLine($"{algorithm.Value}:");
            CalculateHashText(inputBytes, lowerCase, algorithm.Key);
        }
    }

    private void CalculateHashText(byte[] inputBytes, bool lowerCase, HashType hashType)
    {
        var hashedText = CalculateHashType(inputBytes, hashType);

        if (lowerCase)
        {
            hashedText = hashedText.ToLower();
        }

        _outputProvider.AppendLine(hashedText);
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
            HashType.CRC8 => new CRC8Hash(),
            HashType.CRC32 => new CRC32Hash(),
            _ => new MD5Hash()
        };

        var hashBytes = provider.ComputeHash(inputBytes);
        var hashedText = Convert.ToHexString(hashBytes);
        return hashedText;
    }
}