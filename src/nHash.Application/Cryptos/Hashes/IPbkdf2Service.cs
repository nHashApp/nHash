namespace nHash.Application.Cryptos.Hashes;

public class Pbkdf2Result
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public int Iterations { get; set; }
    public string Algorithm { get; set; } = string.Empty;
    public int KeyLength { get; set; }
    public string DerivedKeyHex { get; set; } = string.Empty;
    public string DerivedKeyBase64 { get; set; } = string.Empty;
}

public interface IPbkdf2Service
{
    Pbkdf2Result DeriveKey(string password, string salt, int iterations, string algorithm, int keyLength);
}
