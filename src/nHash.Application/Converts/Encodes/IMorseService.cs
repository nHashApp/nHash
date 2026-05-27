namespace nHash.Application.Encodes;

public interface IMorseService
{
    string Calculate(string text, bool decode);
}
