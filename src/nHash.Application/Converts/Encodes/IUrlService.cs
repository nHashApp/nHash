namespace nHash.Application.Converts.Encodes;

public interface IUrlService
{
    string CalculateTextHash(string text, bool decode);
}