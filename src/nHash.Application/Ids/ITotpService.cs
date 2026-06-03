using nHash.Application.Ids.Models;

namespace nHash.Application.Ids;

public interface ITotpService
{
    TotpGenerateResult Generate(string secretBase32, int digits, int periodSeconds);
    TotpRemainingResult Remaining(int periodSeconds);
}
