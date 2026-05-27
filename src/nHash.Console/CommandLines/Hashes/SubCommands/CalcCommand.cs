using nHash.Application.Hashes;
using nHash.Application.Hashes.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Hashes.SubCommands;

public class CalcCommand(IFileProvider fileProvider, IHashCalcService hashService, IOutputProvider outputProvider) : ICalcCommand
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

    private readonly Option<HashType> _hashType = new("--type", "-t")
    {
        Description = "Hash type (MD5, SHA-1, SHA-256,...)",
        DefaultValueFactory = _ => HashType.All
    };

    private static readonly Dictionary<HashType, string> Algorithms = new()
    {
        { HashType.Md5, "MD5" },
        { HashType.Sha1, "SHA-1" },
        { HashType.Sha256, "SHA-256" },
        { HashType.Sha384, "SHA-384" },
        { HashType.Sha512, "SHA-512" },
        { HashType.Sha3224, "SHA-3 (224)" },
        { HashType.Sha3256, "SHA-3 (256)" },
        { HashType.Sha3384, "SHA-3 (384)" },
        { HashType.Sha3512, "SHA-3 (512)" },
        { HashType.Blake2B, "Blake2b " },
        { HashType.Blake2S, "Blake2s " }
    };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("calc",
            "Calculate hash fingerprint (MD5, SHA-1, SHA-2 (SHA-256, SHA-384, SHA512), SHA-3, Blake, ...)",
            GetExamples());

        command.Options.Add(_fileName);
        command.Options.Add(_lowerCase);
        command.Options.Add(_hashType);
        command.Arguments.Add(_textArgument);

        command.SetAction(async parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var lowerCase = parseResult.GetValue(_lowerCase);
            var fileName = parseResult.GetValue(_fileName);
            var hashType = parseResult.GetValue(_hashType);
            await CalculateText(text ?? string.Empty, lowerCase, fileName ?? string.Empty, hashType);
        });

        command.Aliases.Add("c");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Calculate MD5 hash of a string", "nhash hash calc \"Hello World\" -t md5"),
            new("Calculate SHA-256 hash of a file", "nhash hash calc --file /path/to/file --type sha256"),
            new("Calculate SHA-256 hash of a file", "nhash h c -f /path/to/file -t sha256"),
            new("Calculate all available hash types of a file and write the output to a file",
                "nhash hash calc -f /path/to/file --output /path/to/output/file"),
        ];

    private async Task CalculateText(string text, bool lowerCase, string fileName, HashType hashType)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
            var hashResults = hashService.CalculateText(inputBytes, lowerCase, hashType);
            WriteOutput(hashType, hashResults);
            return;
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var fileBytes = await fileProvider.ReadAsByte(fileName);
            if (fileBytes == Array.Empty<byte>())
            {
                return;
            }

            var hashResults = hashService.CalculateText(fileBytes, lowerCase, hashType);
            WriteOutput(hashType, hashResults);
        }
    }

    private void WriteOutput(HashType hashType, Dictionary<HashType, string> hashResult)
    {
        if (hashType != HashType.All)
        {
            outputProvider.AppendLine(hashResult.First().Value);
            return;
        }

        foreach (var algorithm in hashResult)
        {
            outputProvider.AppendLine($"{Algorithms[algorithm.Key]}:");
            outputProvider.AppendLine(algorithm.Value);
        }
    }
}