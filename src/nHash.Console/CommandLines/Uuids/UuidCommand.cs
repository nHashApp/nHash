using nHash.Application.Uuids;
using nHash.Application.Uuids.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Uuids;

public class UuidCommand : IUuidCommand
{
    public BaseCommand Command => GetCommand();

    private readonly Option<bool> _withBracket;
    private readonly Option<bool> _withoutHyphen;
    private readonly Option<UuidVersion> _version;

    private readonly Dictionary<UuidVersion, string> _uuidLabels = new()
    {
        { UuidVersion.V1, "UUID v1" },
        { UuidVersion.V2, "UUID v2" },
        { UuidVersion.V3, "UUID v3" },
        { UuidVersion.V4, "UUID v4" },
        { UuidVersion.V5, "UUID v5" }
    };

    private readonly IOutputProvider _outputProvider;
    private readonly IUuidService _uuidService;

    public UuidCommand(IOutputProvider outputProvider, IUuidService uuidService)
    {
        _outputProvider = outputProvider;
        _uuidService = uuidService;

        _withBracket = new Option<bool>(name: "--bracket", description: "Generate with brackets");
        _withoutHyphen = new Option<bool>(name: "--no-hyphen", description: "Generate without hyphens");
        _version = new Option<UuidVersion>(name: "--version", () => UuidVersion.All,
            description: "Select UUID version");
        _version.AddAlias("-v");
    }

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("uuid", "Generate a Universally unique identifier (UUID/GUID) version 1 to 5",
            GetExamples())
        {
            _withBracket,
            _withoutHyphen,
            _version
        };
        command.AddAlias("u");
        command.SetHandler(GenerateUuid, _withBracket, _withoutHyphen, _version);

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples()
    {
        return new List<KeyValuePair<string, string>>()
        {
            new("Generate a version 1 UUID with hyphens", "nhash uuid -v v1"),
            new("Generate a version 4 UUID without hyphens", "nhash u -v v4 --no-hyphen"),
            new("Generate all versions of UUIDs with brackets and save the output to a file", "nhash uuid --bracket --output uuid.txt"),
        };
    }

    private void GenerateUuid(bool withBracket, bool withoutHyphen, UuidVersion version)
    {
        var uuidResult = _uuidService.GenerateUuid(withBracket, withBracket, version);
        WriteOutput(version, uuidResult);
    }

    private void WriteOutput(UuidVersion version, Dictionary<UuidVersion, string> uuidResult)
    {
        if (version != UuidVersion.All)
        {
            _outputProvider.AppendLine(uuidResult.First().Value);
            return;
        }

        foreach (var uuid in uuidResult)
        {
            _outputProvider.AppendLine($"{_uuidLabels[uuid.Key]}:");
            _outputProvider.AppendLine($"{uuid.Value}:");
        }
    }
}