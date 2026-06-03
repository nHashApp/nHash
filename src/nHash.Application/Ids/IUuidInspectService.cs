using nHash.Application.Ids.Models;

namespace nHash.Application.Ids;

public interface IUuidInspectService
{
    UuidInspectResult Inspect(string uuid);
}
