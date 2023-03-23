namespace nHash.Application.Encodes;

public interface IBase64Service
{
    void CalculateTextHash(string text, bool decode);
}