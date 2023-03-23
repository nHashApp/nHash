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
        if (!string.IsNullOrWhiteSpace(text))
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
            _hashService.CalculateText(inputBytes, lowerCase, hashType);
            return;
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var fileBytes = await _fileProvider.ReadAsByte(fileName);
            if (fileBytes == Array.Empty<byte>())
            {
                return;
            }

            _hashService.CalculateText(fileBytes, lowerCase, hashType);
        }
    }

    
}