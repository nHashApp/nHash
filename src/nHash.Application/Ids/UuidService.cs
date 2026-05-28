using nHash.Application.Uuids.Models;

namespace nHash.Application.Uuids;

public class UuidService : IUuidService 
{
    private readonly IUuidGenerator _uuidGenerator;
    public UuidService(IUuidGenerator uuidGenerator)
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
        if (version == UuidVersion.Ulid)
        {
            return _uuidGenerator.GenerateUlid();
        }
        if (version == UuidVersion.NanoId)
        {
            return _uuidGenerator.GenerateNanoId();
        }

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
            UuidVersion.V1 => _uuidGenerator.GenerateUuiDv1(),
            UuidVersion.V2 => _uuidGenerator.GenerateUuiDv2(),
            UuidVersion.V3 => _uuidGenerator.GenerateUuiDv3(Guid.NewGuid(), "nHash"),
            UuidVersion.V4 => _uuidGenerator.GenerateUuiDv4(),
            UuidVersion.V5 => _uuidGenerator.GenerateUuiDv5(Guid.NewGuid(), "nHash"),
            UuidVersion.V7 => _uuidGenerator.GenerateUuiDv7(),
            UuidVersion.V8 => _uuidGenerator.GenerateUuiDv8(new byte[16]),
            _ => _uuidGenerator.GenerateUuiDv4()
        };
    }
}