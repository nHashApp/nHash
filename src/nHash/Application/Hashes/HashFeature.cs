using nHash.Application.Hashes.Algorithms;
using nHash.Application.Hashes.Models;

namespace nHash.Application.Hashes;

public class HashFeature : IHashFeature, IFeature
{
    public Command Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument;
    private readonly Option<string> _fileName;
    private readonly Option<bool> _lowerCase;
    private readonly Option<HashType> _hashType;

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

    public HashFeature()
    {
        _textArgument = new Argument<string>("text", () => string.Empty, "Text for calculate fingerprint");
        _fileName = new Option<string>(name: "--file", description: "File name for calculate hash");
        _lowerCase = new Option<bool>(name: "--lower", description: "Generate lower case");
        _hashType = new Option<HashType>(name: "--type", () => HashType.All, "Hash type (MD5, SHA-1, SHA-256,...)");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("hash",
            "Calculate hash fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32, CRC8, ...)")
        {
            _fileName,
            _lowerCase,
            _hashType
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _lowerCase, _fileName, _hashType);

        return command;
    }

    private static async Task CalculateText(string text, bool lowerCase, string fileName, HashType hashType)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
            CalculateHash(inputBytes, lowerCase, hashType);
            return;
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File {fileName} does not exists!");
                return;
            }

            var fileBytes = await File.ReadAllBytesAsync(fileName);
            CalculateHash(fileBytes, lowerCase, hashType);
        }
    }

    private static void CalculateHash(byte[] inputBytes, bool lowerCase, HashType hashType)
    {
        if (hashType != HashType.All)
        {
            CalculateHashText(inputBytes, lowerCase, hashType);
            return;
        }
        
        foreach (var algorithm in Algorithms)
        {
            Console.WriteLine($"{algorithm.Value}:");
            CalculateHashText(inputBytes, lowerCase, algorithm.Key);
        }
    }

    private static void CalculateHashText(byte[] inputBytes, bool lowerCase, HashType hashType)
    {
        var hashedText = CalculateHashType(inputBytes, hashType);

        if (lowerCase)
        {
            hashedText = hashedText.ToLower();
        }

        Console.WriteLine(hashedText);
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