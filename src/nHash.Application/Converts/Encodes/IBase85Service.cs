namespace nHash.Application.Converts.Encodes;

public interface IBase85Service
{
    string Calculate(string text, bool decode);
}
