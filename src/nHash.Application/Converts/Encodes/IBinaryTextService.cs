namespace nHash.Application.Converts.Encodes;

public interface IBinaryTextService
{
    string Calculate(string text, bool decode, int numericBase);
}
