namespace nHash.Application.Encodes;

public interface IBase36Service
{
    string Calculate(string text, bool decode);
}
