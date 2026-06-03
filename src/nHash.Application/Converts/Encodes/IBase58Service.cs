namespace nHash.Application.Converts.Encodes;

public interface IBase58Service
{
    string Calculate(string text, bool decode);
}