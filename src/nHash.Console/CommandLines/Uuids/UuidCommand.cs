using nHash.Application.Uuids;
using nHash.Application.Uuids.Models;

namespace nHash.Console.CommandLines.Uuids;

public class UuidCommand : IUuidCommand
{
    public Command Command => GetCommand();

    private readonly Option<bool> _withBracket = new(name: "--bracket", description: "Generate with brackets");
    private readonly Option<bool> _withoutHyphen = new(name: "--no-hyphen", description: "Generate without hyphens");

    private readonly Option<UuidVersion> _version = new(name: "--version", () => UuidVersion.All,
        description: "Select UUID version");

    private readonly IDictionary<UuidVersion, string> _uuidLabels = new Dictionary<UuidVersion, string>()
    {
        { UuidVersion.V1, "UUID v1" },
        { UuidVersion.V2, "UUID v2" },
        { UuidVersion.V3, "UUID v3" },
        { UuidVersion.V4, "UUID v4" },
        { UuidVersion.V5, "UUID v5" }
    };

    private readonly IUUIDGenerator _uuidGenerator;
    private readonly IOutputProvider _outputProvider;
    private readonly IUuidService _uuidService;

    public UuidCommand(IUUIDGenerator uuidGenerator, IOutputProvider outputProvider, IUuidService uuidService)
    {
        _uuidGenerator = uuidGenerator;
        _outputProvider = outputProvider;
        _uuidService = uuidService;
    }

    private Command GetCommand()
    {
        var command = new Command("uuid", "Generate a Universally unique identifier (UUID/GUID) version 1 to 5")
        {
            _withBracket,
            _withoutHyphen,
            _version
        };
        command.SetHandler(GenerateUuid, _withBracket, _withoutHyphen, _version);

        return command;
    }

    private void GenerateUuid(bool withBracket, bool withoutHyphen, UuidVersion version)
    {
        _uuidService.GenerateUuid(withBracket, withBracket, version);
    }
}