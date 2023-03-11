using System.Security.Cryptography;
using nHash.Features.Models;

namespace nHash.Features;

public class HashAlgorithmFeature : IFeature
{
    public Command Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument;
    private readonly Argument<AlgorithmType> _algorithmType;
    private readonly Option<string> _fileName;
    private readonly Option<bool> _lowerCase;


    public HashAlgorithmFeature()
    {
        _algorithmType = new Argument<AlgorithmType>("type", "Algorithm type");
        _textArgument = new Argument<string>("text", GetDefaultString, "Text for calculate fingerprint");
        _fileName = new Option<string>(name: "--file", description: "File name for calculate hash");
        _lowerCase = new Option<bool>(name: "--lower", description: "Generate lower case");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("hash",
            "Calculate hash fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512)")
        {
            _fileName,
            _lowerCase,
        };
        command.AddArgument(_algorithmType);
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _algorithmType, _lowerCase, _fileName);

        return command;
    }

    private static void CalculateText(string text, AlgorithmType algorithmType, bool lowerCase, string fileName)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
            CalculateHash(inputBytes, algorithmType, lowerCase);
            return;
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File {fileName} does not exists!");
                return;
            }
            
            var fileBytes = File.ReadAllBytes(fileName);
            CalculateHash(fileBytes, algorithmType, lowerCase);
        }
    }

    private static void CalculateHash(byte[] inputBytes, AlgorithmType algorithmType, bool lowerCase)
    {
        HashAlgorithm provider = algorithmType switch
        {
            AlgorithmType.MD5 => MD5.Create(),
            AlgorithmType.SHA1 => SHA1.Create(),
            AlgorithmType.SHA256 => SHA256.Create(),
            AlgorithmType.SHA384 => SHA384.Create(),
            AlgorithmType.SHA512 => SHA512.Create(),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithmType), algorithmType, null)
        };

        var hashBytes = provider.ComputeHash(inputBytes);
        var hashedText = Convert.ToHexString(hashBytes);

        if (lowerCase)
        {
            hashedText = hashedText.ToLower();
        }

        Console.WriteLine(hashedText);
    }

    private static string GetDefaultString() => string.Empty;
}