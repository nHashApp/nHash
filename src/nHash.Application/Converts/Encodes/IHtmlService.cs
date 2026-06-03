namespace nHash.Application.Converts.Encodes;

public interface IHtmlService
{
    string CalculateTextHash(string text, bool decode);
}