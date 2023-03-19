using nHash.Application.Uuids.Models;

namespace nHash.Application.Uuids;

public class UuidFeature : IUuidFeature, IFeature 
{
    private readonly IUUIDGenerator _uuidGenerator = new UUIDGenerator();
    
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
        if (version != UuidVersion.All)
        {
            GenerateUuidText(withBracket, withoutHyphen, version);
            return;
        }

        foreach (var uuidLabel in _uuidLabels)
        {
            Console.WriteLine(uuidLabel.Value + ":");
            GenerateUuidText(withBracket, withoutHyphen, uuidLabel.Key);
        }
    }

    private void GenerateUuidText(bool withBracket, bool withoutHyphen, UuidVersion version)
    {
        var guid = GenerateUuidByVersion(version);
        var result = withBracket ? guid.ToString("B") : guid.ToString();
        if (withoutHyphen)
        {
            result = result.Replace("-", "");
        }

        Console.WriteLine(result);
    }

    private Guid GenerateUuidByVersion(UuidVersion version)
    {
        return version switch
        {
            UuidVersion.V1 => _uuidGenerator.GenerateUUIDv1(),
            UuidVersion.V2 => _uuidGenerator.GenerateUUIDv2(),
            UuidVersion.V3 => _uuidGenerator.GenerateUUIDv3(Guid.NewGuid(), "nHash"),
            UuidVersion.V4 => _uuidGenerator.GenerateUUIDv4(),
            UuidVersion.V5 => _uuidGenerator.GenerateUUIDv5(Guid.NewGuid(), "nHash"),
            _ => _uuidGenerator.GenerateUUIDv4()
        };
    }
}