namespace nHash.Application.Encodes;

public interface IRot13Service
{
    string Calculate(string text, int shift);
}
