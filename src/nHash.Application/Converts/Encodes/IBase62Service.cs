namespace nHash.Application.Encodes;

public interface IBase62Service
{
    string Calculate(string text, bool decode);
}
