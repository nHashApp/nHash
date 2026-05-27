namespace nHash.Application.Encodes;

public interface IBinaryTextService
{
    string Calculate(string text, bool decode, int numericBase);
}
