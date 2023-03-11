using System.Security.Cryptography;

namespace nHash.Features;

public class HashAlgorithmFeature : IFeature
{
    public Command Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument;
    private readonly Option<string> _fileName;
    private readonly Option<bool> _lowerCase;


    public HashAlgorithmFeature()
    {
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
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _lowerCase, _fileName);

        return command;
    }

    private static void CalculateText(string text, bool lowerCase, string fileName)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
            CalculateHash(inputBytes, lowerCase);
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
            CalculateHash(fileBytes, lowerCase);
        }
    }

    private static void CalculateHash(byte[] inputBytes, bool lowerCase)
    {
        var algorithms = new Dictionary<string, HashAlgorithm>()
        {
            { "MD5", MD5.Create() },
            { "SHA-1", SHA1.Create() },
            { "SHA-256", SHA256.Create() },
            { "SHA-384", SHA384.Create() },
            { "SHA-512", SHA512.Create() }
        };

        foreach (var algorithm in algorithms)
        {
            var hashBytes = algorithm.Value.ComputeHash(inputBytes);
            var hashedText = Convert.ToHexString(hashBytes);

            if (lowerCase)
            {
                hashedText = hashedText.ToLower();
            }

            Console.WriteLine($"{algorithm.Key}:");
            Console.WriteLine(hashedText);
        }
    }

    private static string GetDefaultString() => string.Empty;
}