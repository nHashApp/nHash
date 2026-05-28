namespace nHash.Application.Encodes;

public interface IBase85Service
{
    string Calculate(string text, bool decode);
}
