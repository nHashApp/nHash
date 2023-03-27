using nHash.Application.Hashes;
using nHash.Application.Hashes.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Hashes.SubCommands;

public class CalcCommand : ICalcCommand
{
    public BaseCommand Command => GetFeatureCommand();
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
        { HashType.SHA3_224, "SHA-3 (224)" },
        { HashType.SHA3_256, "SHA-3 (256)" },
        { HashType.SHA3_384, "SHA-3 (384)" },
        { HashType.SHA3_512, "SHA-3 (512)" },
        { HashType.BLAKE2b, "Blake2b " },
        { HashType.BLAKE2s, "Blake2s " }
    };

    private readonly IFileProvider _fileProvider;
    private readonly IHashCalcService _hashService;
    private readonly IOutputProvider _outputProvider;

    public CalcCommand(IFileProvider fileProvider, IHashCalcService hashService, IOutputProvider outputProvider)
    {
        _fileProvider = fileProvider;
        _hashService = hashService;
        _outputProvider = outputProvider;
        _textArgument = new Argument<string>("text", () => string.Empty, "Text for calculate fingerprint");
        _fileName = new Option<string>(name: "--file", description: "File name for calculate hash");
        _fileName.AddAlias("-f");
        _lowerCase = new Option<bool>(name: "--lower", description: "Generate lower case");
        _hashType = new Option<HashType>(name: "--type", () => HashType.All, "Hash type (MD5, SHA-1, SHA-256,...)");
        _hashType.AddAlias("-t");
    }

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("calc",
            "Calculate hash fingerprint (MD5, SHA-1, SHA-2 (SHA-256, SHA-384, SHA512), SHA-3, Blake, ...)",
            GetExamples())
        {
            _fileName,
            _lowerCase,
            _hashType
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _lowerCase, _fileName, _hashType);
        command.AddAlias("c");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples()
    {
        return new List<KeyValuePair<string, string>>()
        {
            new("Calculate MD5 hash of a string", "nhash hash calc \"Hello World\" -t md5"),
            new("Calculate SHA-256 hash of a file", "nhash hash calc --file /path/to/file --type sha256"),
            new("Calculate SHA-256 hash of a file", "nhash h c -f /path/to/file -t sha256"),
            new("Calculate all available hash types of a file and write the output to a file",
                "nhash hash calc -f /path/to/file --output /path/to/output/file"),
        };
    }

    private async Task CalculateText(string text, bool lowerCase, string fileName, HashType hashType)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
            var hashResults = _hashService.CalculateText(inputBytes, lowerCase, hashType);
            WriteOutput(hashType, hashResults);
            return;
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var fileBytes = await _fileProvider.ReadAsByte(fileName);
            if (fileBytes == Array.Empty<byte>())
            {
                return;
            }

            var hashResults = _hashService.CalculateText(fileBytes, lowerCase, hashType);
            WriteOutput(hashType, hashResults);
        }
    }

    private void WriteOutput(HashType hashType, Dictionary<HashType, string> hashResult)
    {
        if (hashType != HashType.All)
        {
            _outputProvider.AppendLine(hashResult.First().Value);
            return;
        }

        foreach (var algorithm in hashResult)
        {
            _outputProvider.AppendLine($"{Algorithms[algorithm.Key]}:");
            _outputProvider.AppendLine(algorithm.Value);
        }
    }
}