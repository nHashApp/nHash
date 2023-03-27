namespace nHash.Application.Encodes;

public interface IBase58Service
{
    string Calculate(string text, bool decode);
}