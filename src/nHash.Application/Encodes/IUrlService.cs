namespace nHash.Application.Encodes;

public interface IUrlService
{
    string CalculateTextHash(string text, bool decode);
}