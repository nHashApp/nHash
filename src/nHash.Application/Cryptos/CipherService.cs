using System.Security.Cryptography;
using System.Text;

namespace nHash.Application.Cryptos;

public class CipherService : ICipherService
{
    private static readonly byte[] Salt = "nHashSalt9876543"u8.ToArray();

    public string Encrypt(string plainText, string password, string algorithm)
    {
        var key = DeriveKey(password);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);

        var nonce = new byte[12];
        RandomNumberGenerator.Fill(nonce);

        var tag = new byte[16];
        var cipherBytes = new byte[plainBytes.Length];

        if (algorithm.ToLowerInvariant().Contains("chacha"))
        {
            using var chacha = new ChaCha20Poly1305(key);
            chacha.Encrypt(nonce, plainBytes, cipherBytes, tag);
        }
        else
        {
            using var aes = new AesGcm(key, 16);
            aes.Encrypt(nonce, plainBytes, cipherBytes, tag);
        }

        var result = new byte[12 + 16 + cipherBytes.Length];
        Array.Copy(nonce, 0, result, 0, 12);
        Array.Copy(tag, 0, result, 12, 16);
        Array.Copy(cipherBytes, 0, result, 28, cipherBytes.Length);

        return Convert.ToHexString(result);
    }

    public string Decrypt(string cipherText, string password, string algorithm)
    {
        try
        {
            var key = DeriveKey(password);
            var cipherBytesAll = Convert.FromHexString(cipherText);

            if (cipherBytesAll.Length < 28)
            {
                return "Invalid cipher text length.";
            }

            var nonce = new byte[12];
            var tag = new byte[16];
            var cipherBytes = new byte[cipherBytesAll.Length - 28];

            Array.Copy(cipherBytesAll, 0, nonce, 0, 12);
            Array.Copy(cipherBytesAll, 12, tag, 0, 16);
            Array.Copy(cipherBytesAll, 28, cipherBytes, 0, cipherBytes.Length);

            var plainBytes = new byte[cipherBytes.Length];

            if (algorithm.ToLowerInvariant().Contains("chacha"))
            {
                using var chacha = new ChaCha20Poly1305(key);
                chacha.Decrypt(nonce, cipherBytes, tag, plainBytes);
            }
            else
            {
                using var aes = new AesGcm(key, 16);
                aes.Decrypt(nonce, cipherBytes, tag, plainBytes);
            }

            return Encoding.UTF8.GetString(plainBytes);
        }
        catch (Exception)
        {
            return "Decryption failed. Please verify your password and ciphertext.";
        }
    }

    private static byte[] DeriveKey(string password)
    {
        return Rfc2898DeriveBytes.Pbkdf2(password, Salt, 10000, HashAlgorithmName.SHA256, 32);
    }
}
