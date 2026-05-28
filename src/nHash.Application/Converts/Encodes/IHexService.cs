namespace nHash.Application.Encodes;

public interface IHexService
{
    string Calculate(string text, bool decode);
}
