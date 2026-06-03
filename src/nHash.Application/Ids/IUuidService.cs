using nHash.Application.Ids.Models;

namespace nHash.Application.Ids;

public interface IUuidService
{
    Dictionary<UuidVersion,string> GenerateUuid(bool withBracket, bool withoutHyphen, UuidVersion version);
}