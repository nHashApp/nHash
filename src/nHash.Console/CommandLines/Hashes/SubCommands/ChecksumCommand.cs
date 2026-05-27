using nHash.Application.Hashes;
using nHash.Application.Hashes.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Hashes.SubCommands;

public class ChecksumCommand(IFileProvider fileProvider, IChecksumService hashService, IOutputProvider outputProvider) : IChecksumCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text")
    {
        Description = "Text for calculate fingerprint",
        DefaultValueFactory = _ => string.Empty
    };

    private readonly Option<string> _fileName = new("--file", "-f")
    {
        Description = "File name for calculate hash"
    };

    private readonly Option<bool> _lowerCase = new("--lower")
    {
        Description = "Generate lower case"
    };

    private readonly Option<string> _verify = new("--verify", "-v")
    {
        Description = "Use the checksum type provided to verify your checksum"
    };

    private readonly Option<ChecksumType> _hashType = new("--type", "-t")
    {
        Description = "Hash type (MD5, SHA-1, CRC-8, CRC-32, Adler-32,...)",
        DefaultValueFactory = _ => ChecksumType.All
    };

    private static readonly Dictionary<ChecksumType, string> Algorithms = new()
    {
        { ChecksumType.Md5, "MD5" },
        { ChecksumType.Sha1, "SHA-1" },
        { ChecksumType.Crc8, "CRC-8" },
        { ChecksumType.Crc32, "CRC-32" },
        { ChecksumType.Adler32, "Adler-32" },
        { ChecksumType.Fletcher16, "Fletcher-16" },
        { ChecksumType.Fletcher32, "Fletcher-32" }
    };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("checksum",
            "Calculate checksum fingerprint (MD5, SHA-1, CRC32, CRC8, Adler-32,...)", GetExamples());

        command.Options.Add(_fileName);
        command.Options.Add(_lowerCase);
        command.Options.Add(_hashType);
        command.Options.Add(_verify);
        command.Arguments.Add(_textArgument);

        command.SetAction(async parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var lowerCase = parseResult.GetValue(_lowerCase);
            var fileName = parseResult.GetValue(_fileName);
            var hashType = parseResult.GetValue(_hashType);
            var verify = parseResult.GetValue(_verify);
            await CalculateText(text ?? string.Empty, lowerCase, fileName ?? string.Empty, hashType, verify ?? string.Empty);
        });

        command.Aliases.Add("ch");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Calculate the MD5 checksum of a given text", "nhash hash checksum \"Hello, World\" -t md5"),
            new("Calculate the CRC-8 checksum of a file", "nhash h ch -f /path/to/file.txt -t crc8"),
            new("Verify a checksum", "nhash hash checksum \"Hello, World\" -t md5 -v 82BB413746AEE42F89DEA2B59614F9EF"),
            new("Calculate multiple checksums at once", "nhash hash checksum -f /path/to/file.txt -t all"),
        ];

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
            inputBytes = await fileProvider.ReadAsByte(fileName);
        }

        if (inputBytes == Array.Empty<byte>())
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(targetChecksum))
        {
            var hashResults = hashService.CalculateText(inputBytes, lowerCase, hashType);
            WriteHashOutput(hashType, hashResults);
        }
        else
        {
            var verifyResult = hashService.VerifyChecksum(inputBytes, targetChecksum, hashType);
            WriteVerifyOutput(targetChecksum, verifyResult.NewChecksum, verifyResult.IsMatch);
        }
    }

    private void WriteHashOutput(ChecksumType hashType, Dictionary<ChecksumType, string> hashResult)
    {
        if (hashType != ChecksumType.All)
        {
            outputProvider.AppendLine(hashResult.First().Value);
            return;
        }

        foreach (var algorithm in hashResult)
        {
            outputProvider.AppendLine($"{Algorithms[algorithm.Key]}:");
            outputProvider.AppendLine(algorithm.Value);
            outputProvider.AppendLine();
        }
    }

    private void WriteVerifyOutput(string targetChecksum, string newChecksum, bool isMatch)
    {
        outputProvider.AppendLine($"Your checksum= {targetChecksum}");
        outputProvider.AppendLine($"Data checksum= {newChecksum}");
        outputProvider.AppendLine($"Result is: {(isMatch ? "Match" : "Not Match")}");
    }
}