using System.Security.Cryptography;
using System.Text;

namespace nHash.Application.Cryptos.Hashes;

public class Pbkdf2Service : IPbkdf2Service
{
    public Pbkdf2Result DeriveKey(string password, string salt, int iterations, string algorithm, int keyLength)
    {
        var result = new Pbkdf2Result
        {
            Password = password ?? string.Empty,
            Salt = salt ?? string.Empty,
            Iterations = iterations,
            Algorithm = algorithm ?? "sha256",
            KeyLength = keyLength
        };

        if (password == null)
        {
            result.Success = false;
            result.ErrorMessage = "Password cannot be null.";
            return result;
        }

        if (salt == null)
        {
            result.Success = false;
            result.ErrorMessage = "Salt cannot be null.";
            return result;
        }

        if (iterations <= 0)
        {
            result.Success = false;
            result.ErrorMessage = "Iterations must be greater than 0.";
            return result;
        }

        if (keyLength <= 0)
        {
            result.Success = false;
            result.ErrorMessage = "Key length must be greater than 0.";
            return result;
        }

        var hashAlgorithm = (algorithm ?? "").ToLowerInvariant().Trim() switch
        {
            "sha1" => HashAlgorithmName.SHA1,
            "sha256" or "sha-256" => HashAlgorithmName.SHA256,
            "sha384" or "sha-384" => HashAlgorithmName.SHA384,
            "sha512" or "sha-512" => HashAlgorithmName.SHA512,
            _ => HashAlgorithmName.SHA256
        };

        try
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            var derivedBytes = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, iterations, hashAlgorithm, keyLength);

            result.DerivedKeyHex = Convert.ToHexString(derivedBytes).ToLowerInvariant();
            result.DerivedKeyBase64 = Convert.ToBase64String(derivedBytes);
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"PBKDF2 Derivation Error: {ex.Message}";
            return result;
        }
    }
}
