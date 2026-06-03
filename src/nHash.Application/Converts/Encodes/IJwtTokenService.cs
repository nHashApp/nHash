using nHash.Application.Converts.Encodes.Models;

namespace nHash.Application.Converts.Encodes;

public interface IJwtTokenService
{
    JwtTokenResponse DecodeJwtToken(string text, bool noWriteInformation);
}