using nHash.Providers.Hashing;

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
            "Calculate hash fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32, CRC64, CRC, ...)")
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
        var algorithms = new Dictionary<string, IHash>()
        {
            { "MD5", new MD5Hash() },
            { "SHA-1", new SHA1Hash() },
            { "SHA-256", new SHA256Hash() },
            { "SHA-384", new SHA384Hash() },
            { "SHA-512", new SHA512Hash() },
            { "CRC-8", new CRC8Hash() },
            { "CRC-32", new CRC32Hash() },
        };

        foreach (var algorithm in algorithms)
        {
            var hashBytes = algorithm.Value.ComputeHash(inputBytes);
            var hashedText = Convert.ToHexString(hashBytes);
            //var hashedText = BitConverter.ToString(hashBytes).Replace("-","");
            
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