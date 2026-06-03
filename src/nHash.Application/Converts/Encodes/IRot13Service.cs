namespace nHash.Application.Converts.Encodes;

public interface IRot13Service
{
    string Calculate(string text, int shift);
}
