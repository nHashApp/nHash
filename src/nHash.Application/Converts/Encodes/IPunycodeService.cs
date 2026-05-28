namespace nHash.Application.Encodes;

public interface IPunycodeService
{
    string Calculate(string text, bool decode);
}
