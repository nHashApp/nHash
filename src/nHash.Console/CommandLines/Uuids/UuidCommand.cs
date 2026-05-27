using nHash.Application.Uuids;
using nHash.Application.Uuids.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Uuids;

public class UuidCommand(IOutputProvider outputProvider, IUuidService uuidService) : IUuidCommand
{
    public BaseCommand Command => GetCommand();

    private readonly Option<bool> _withBracket = new("--bracket") { Description = "Generate with brackets" };
    private readonly Option<bool> _withoutHyphen = new("--no-hyphen") { Description = "Generate without hyphens" };
    private readonly Option<UuidVersion> _version = new("--version", "-v") { Description = "Select UUID version", DefaultValueFactory = _ => UuidVersion.All };

    private readonly Dictionary<UuidVersion, string> _uuidLabels = new()
    {
        { UuidVersion.V1, "UUID v1" },
        { UuidVersion.V2, "UUID v2" },
        { UuidVersion.V3, "UUID v3" },
        { UuidVersion.V4, "UUID v4" },
        { UuidVersion.V5, "UUID v5" }
    };

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("uuid", "Generate a Universally unique identifier (UUID/GUID) version 1 to 5",
            GetExamples());

        command.Options.Add(_withBracket);
        command.Options.Add(_withoutHyphen);
        command.Options.Add(_version);

        command.Aliases.Add("u");
        command.SetAction(parseResult =>
        {
            var withBracket = parseResult.GetValue(_withBracket);
            var withoutHyphen = parseResult.GetValue(_withoutHyphen);
            var version = parseResult.GetValue(_version);
            GenerateUuid(withBracket, withoutHyphen, version);
        });

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Generate a version 1 UUID with hyphens", "nhash uuid -v v1"),
            new("Generate a version 4 UUID without hyphens", "nhash u -v v4 --no-hyphen"),
            new("Generate all versions of UUIDs with brackets and save the output to a file", "nhash uuid --bracket --output uuid.txt"),
        ];

    private void GenerateUuid(bool withBracket, bool withoutHyphen, UuidVersion version)
    {
        var uuidResult = uuidService.GenerateUuid(withBracket, withoutHyphen, version);
        WriteOutput(version, uuidResult);
    }

    private void WriteOutput(UuidVersion version, Dictionary<UuidVersion, string> uuidResult)
    {
        if (version != UuidVersion.All)
        {
            outputProvider.AppendLine(uuidResult.First().Value);
            return;
        }

        foreach (var uuid in uuidResult)
        {
            outputProvider.AppendLine($"{_uuidLabels[uuid.Key]}:");
            outputProvider.AppendLine($"{uuid.Value}:");
        }
    }
}