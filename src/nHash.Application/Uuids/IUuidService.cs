using nHash.Application.Uuids.Models;

namespace nHash.Application.Uuids;

public interface IUuidService
{
    Dictionary<UuidVersion,string> GenerateUuid(bool withBracket, bool withoutHyphen, UuidVersion version);
}