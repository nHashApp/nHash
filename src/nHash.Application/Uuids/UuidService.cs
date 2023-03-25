using nHash.Application.Uuids.Models;

namespace nHash.Application.Uuids;

public class UuidService : IUuidService 
{
    private readonly IUUIDGenerator _uuidGenerator;
    public UuidService(IUUIDGenerator uuidGenerator)
    {
        _uuidGenerator = uuidGenerator;
    }

    public Dictionary<UuidVersion,string> GenerateUuid(bool withBracket, bool withoutHyphen, UuidVersion version)
    {
        var result = new Dictionary<UuidVersion, string>();
        if (version != UuidVersion.All)
        {
            var returnedUuid =GenerateUuidText(withBracket, withoutHyphen, version);
            result.Add(version, returnedUuid);
            return result;
        }

        foreach (var uuid in Enum.GetValues<UuidVersion>())
        {
            if (uuid == UuidVersion.All)
            {
                continue;
            }
            
            var returnedUuid =GenerateUuidText(withBracket, withoutHyphen, uuid);
            result.Add(uuid, returnedUuid);
        }

        return result;
    }

    private string GenerateUuidText(bool withBracket, bool withoutHyphen, UuidVersion version)
    {
        var guid = GenerateUuidByVersion(version);
        var result = withBracket ? guid.ToString("B") : guid.ToString();
        if (withoutHyphen)
        {
            result = result.Replace("-", "");
        }

        return result;
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