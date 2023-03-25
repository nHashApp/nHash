namespace nHash.Application.Encodes;

public interface IBase64Service
{
    string CalculateTextHash(string text, bool decode);
}