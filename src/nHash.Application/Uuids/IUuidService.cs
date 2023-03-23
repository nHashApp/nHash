using nHash.Application.Uuids.Models;

namespace nHash.Application.Uuids;

public interface IUuidService
{
    void GenerateUuid(bool withBracket, bool withoutHyphen, UuidVersion version);
}