namespace nHash.Application.Encodes;

public interface IHtmlService
{
    void CalculateTextHash(string text, bool decode);
}