namespace nHash.Application.Encodes;

public interface IBase64Service
{
    string Calculate(string text, bool decode);
}