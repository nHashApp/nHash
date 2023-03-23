using nHash.Application.Hashes;
using nHash.Application.Hashes.Models;

namespace nHash.Console.CommandLines.Hashes;

public class HashCommand : IHashCommand
{
    public Command Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument;
    private readonly Option<string> _fileName;
    private readonly Option<bool> _lowerCase;
    private readonly Option<HashType> _hashType;

    private readonly IFileProvider _fileProvider;
    private readonly IHashService _hashService;

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

    public HashCommand(IFileProvider fileProvider, IHashService hashService)
    {
        _fileProvider = fileProvider;
        _hashService = hashService;
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

    private async Task CalculateText(string text, bool lowerCase, string fileName, HashType hashType)
    {
        await _hashService.CalculateText(text, lowerCase, fileName, hashType);
    }

    
}