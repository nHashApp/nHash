using nHash.Application.Abstraction;
using nHash.Application.Uuids.Models;

namespace nHash.Application.Uuids;

public class UuidService : IUuidService 
{

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
    public UuidService(IUUIDGenerator uuidGenerator, IOutputProvider outputProvider)
    {
        _uuidGenerator = uuidGenerator;
        _outputProvider = outputProvider;
    }

    public void GenerateUuid(bool withBracket, bool withoutHyphen, UuidVersion version)
    {
        if (version != UuidVersion.All)
        {
            GenerateUuidText(withBracket, withoutHyphen, version);
            return;
        }

        foreach (var uuidLabel in _uuidLabels)
        {
            _outputProvider.AppendLine(uuidLabel.Value + ":");
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

        _outputProvider.AppendLine(result);
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