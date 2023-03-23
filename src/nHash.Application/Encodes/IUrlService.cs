namespace nHash.Application.Encodes;

public interface IUrlService
{
    void CalculateTextHash(string text, bool decode);
}