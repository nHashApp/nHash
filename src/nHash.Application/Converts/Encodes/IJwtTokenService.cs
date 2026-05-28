using nHash.Application.Encodes.Models;

namespace nHash.Application.Encodes;

public interface IJwtTokenService
{
    JwtTokenResponse DecodeJwtToken(string text, bool noWriteInformation);
}