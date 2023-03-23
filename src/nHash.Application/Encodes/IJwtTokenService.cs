namespace nHash.Application.Encodes;

public interface IJwtTokenService
{
    void DecodeJwtToken(string text, bool noWriteInformation);
}