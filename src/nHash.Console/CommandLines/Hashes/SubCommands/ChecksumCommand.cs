using nHash.Application.Hashes;
using nHash.Application.Hashes.Models;

namespace nHash.Console.CommandLines.Hashes.SubCommands;

public class ChecksumCommand : IChecksumCommand
{
    public Command Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument;
    private readonly Option<string> _fileName;
    private readonly Option<bool> _lowerCase;
    private readonly Option<string> _verify;
    private readonly Option<ChecksumType> _hashType;

    private static readonly Dictionary<ChecksumType, string> Algorithms = new()
    {
        { ChecksumType.MD5, "MD5" },
        { ChecksumType.SHA1, "SHA-1" },
        { ChecksumType.CRC8, "CRC-8" },
        { ChecksumType.CRC32, "CRC-32" },
        { ChecksumType.Adler32, "Adler-32" },
        { ChecksumType.Fletcher16, "Fletcher-16" },
        { ChecksumType.Fletcher32, "Fletcher-32" }
    };

    private readonly IFileProvider _fileProvider;
    private readonly IChecksumService _hashService;
    private readonly IOutputProvider _outputProvider;

    public ChecksumCommand(IFileProvider fileProvider, IChecksumService hashService, IOutputProvider outputProvider)
    {
        _fileProvider = fileProvider;
        _hashService = hashService;
        _outputProvider = outputProvider;
        _textArgument = new Argument<string>("text", () => string.Empty, "Text for calculate fingerprint");
        _fileName = new Option<string>(name: "--file", description: "File name for calculate hash");
        _lowerCase = new Option<bool>(name: "--lower", description: "Generate lower case");
        _hashType = new Option<ChecksumType>(name: "--type", () => ChecksumType.All,
            "Hash type (MD5, SHA-1, CRC-8, CRC-32, Adler-32,...)");
        _verify = new Option<string>(name: "--verify",
            description: "Use the checksum type provided to verify your checksum");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("checksum",
            "Calculate checksum fingerprint (MD5, SHA-1, CRC32, CRC8, Adler-32,...)")
        {
            _fileName,
            _lowerCase,
            _hashType,
            _verify
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _lowerCase, _fileName, _hashType, _verify);

        return command;
    }

    private async Task CalculateText(string text, bool lowerCase, string fileName, ChecksumType hashType,
        string targetChecksum)
    {
        if (!string.IsNullOrWhiteSpace(targetChecksum) && hashType == ChecksumType.All)
        {
            System.Console.WriteLine(
                "The hash type must be selected and can't be All. The --type option must be used to select the type");
            return;
        }

        var inputBytes = Array.Empty<byte>();
        if (!string.IsNullOrWhiteSpace(text))
        {
            inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            inputBytes = await _fileProvider.ReadAsByte(fileName);
        }

        if (inputBytes == Array.Empty<byte>())
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(targetChecksum))
        {
            var hashResults = _hashService.CalculateText(inputBytes, lowerCase, hashType);
            WriteHashOutput(hashType, hashResults);
        }
        else
        {
            var verifyResult = _hashService.VerifyChecksum(inputBytes, targetChecksum, hashType);
            WriteVerifyOutput(targetChecksum, verifyResult.NewChecksum, verifyResult.IsMatch);
        }
    }

    private void WriteHashOutput(ChecksumType hashType, Dictionary<ChecksumType, string> hashResult)
    {
        if (hashType != ChecksumType.All)
        {
            _outputProvider.AppendLine(hashResult.First().Value);
            return;
        }

        foreach (var algorithm in hashResult)
        {
            _outputProvider.AppendLine($"{Algorithms[algorithm.Key]}:");
            _outputProvider.AppendLine(algorithm.Value);
            _outputProvider.AppendLine();
        }
    }

    private void WriteVerifyOutput(string targetChecksum, string newChecksum, bool isMatch)
    {
        _outputProvider.AppendLine($"Your checksum= {targetChecksum}");
        _outputProvider.AppendLine($"Data checksum= {newChecksum}");
        _outputProvider.AppendLine($"Result is: {(isMatch ? "Match" : "Not Match")}");
    }
}