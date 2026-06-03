namespace nHash.Application.Cryptos.Passwords;

public class BCryptHashResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
}

public class BCryptVerifyResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsValid { get; set; }
}

public interface IBCryptService
{
    BCryptHashResult Hash(string password, int workFactor);
    BCryptVerifyResult Verify(string password, string hashedPassword);
}
