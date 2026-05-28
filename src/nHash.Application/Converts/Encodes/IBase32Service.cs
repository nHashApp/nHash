namespace nHash.Application.Encodes;

public interface IBase32Service
{
    string Calculate(string text, bool decode);
}
