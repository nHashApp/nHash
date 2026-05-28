namespace nHash.Application.Ids;

public interface ITotpService
{
    string Generate(string secretBase32, int digits, int periodSeconds);
    string Remaining(int periodSeconds);
}
