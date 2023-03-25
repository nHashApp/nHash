namespace nHash.Application.Encodes;

public interface IHtmlService
{
    string CalculateTextHash(string text, bool decode);
}