using System;
using System.Security.Cryptography;
using System.Text;

namespace nHash.Application.Cryptos;

public class CipherService : ICipherService
{
    private static readonly byte[] DefaultSalt = "nHashSalt9876543"u8.ToArray();

    public string Encrypt(string plainText, string password, string algorithm)
    {
        algorithm = string.IsNullOrWhiteSpace(algorithm) ? "aes" : algorithm;
        var salt = new byte[16];
        RandomNumberGenerator.Fill(salt);

        var key = DeriveKey(password, salt);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);

        var isCbc = algorithm.ToLowerInvariant().Contains("cbc");
        if (isCbc)
        {
            var iv = new byte[16];
            RandomNumberGenerator.Fill(iv);

            byte[] encrypted;
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using var encryptor = aes.CreateEncryptor();
                encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }

            var result = new byte[16 + 16 + encrypted.Length];
            Array.Copy(salt, 0, result, 0, 16);
            Array.Copy(iv, 0, result, 16, 16);
            Array.Copy(encrypted, 0, result, 32, encrypted.Length);

            return Convert.ToHexString(result);
        }

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

        var resultGcm = new byte[16 + 12 + 16 + cipherBytes.Length];
        Array.Copy(salt, 0, resultGcm, 0, 16);
        Array.Copy(nonce, 0, resultGcm, 16, 12);
        Array.Copy(tag, 0, resultGcm, 28, 16);
        Array.Copy(cipherBytes, 0, resultGcm, 44, cipherBytes.Length);

        return Convert.ToHexString(resultGcm);
    }

    public string Decrypt(string cipherText, string password, string algorithm)
    {
        algorithm = string.IsNullOrWhiteSpace(algorithm) ? "aes" : algorithm;
        try
        {
            var cipherBytesAll = Convert.FromHexString(cipherText);

            var isCbc = algorithm.ToLowerInvariant().Contains("cbc");
            if (isCbc)
            {
                if (cipherBytesAll.Length < 32)
                {
                    return "Invalid cipher text length.";
                }

                var salt = new byte[16];
                var iv = new byte[16];
                var cipherBytes = new byte[cipherBytesAll.Length - 32];

                Array.Copy(cipherBytesAll, 0, salt, 0, 16);
                Array.Copy(cipherBytesAll, 16, iv, 0, 16);
                Array.Copy(cipherBytesAll, 32, cipherBytes, 0, cipherBytes.Length);

                var key = DeriveKey(password, salt);

                using var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using var decryptor = aes.CreateDecryptor();
                var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }

            if (cipherBytesAll.Length < 28)
            {
                return "Invalid cipher text length.";
            }

            if (cipherBytesAll.Length >= 44)
            {
                try
                {
                    return DecryptNewFormat(cipherBytesAll, password, algorithm);
                }
                catch (CryptographicException)
                {
                    // Fallback to old format
                }
            }

            return DecryptOldFormat(cipherBytesAll, password, algorithm);
        }
        catch (Exception)
        {
            return "Decryption failed. Please verify your password and ciphertext.";
        }
    }

    private static string DecryptNewFormat(byte[] cipherBytesAll, string password, string algorithm)
    {
        var salt = new byte[16];
        var nonce = new byte[12];
        var tag = new byte[16];
        var cipherBytes = new byte[cipherBytesAll.Length - 44];

        Array.Copy(cipherBytesAll, 0, salt, 0, 16);
        Array.Copy(cipherBytesAll, 16, nonce, 0, 12);
        Array.Copy(cipherBytesAll, 28, tag, 0, 16);
        Array.Copy(cipherBytesAll, 44, cipherBytes, 0, cipherBytes.Length);

        var key = DeriveKey(password, salt);
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

    private static string DecryptOldFormat(byte[] cipherBytesAll, string password, string algorithm)
    {
        var nonce = new byte[12];
        var tag = new byte[16];
        var cipherBytes = new byte[cipherBytesAll.Length - 28];

        Array.Copy(cipherBytesAll, 0, nonce, 0, 12);
        Array.Copy(cipherBytesAll, 12, tag, 0, 16);
        Array.Copy(cipherBytesAll, 28, cipherBytes, 0, cipherBytes.Length);

        var key = DeriveKey(password, DefaultSalt);
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

    private static byte[] DeriveKey(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(password, salt, 10000, HashAlgorithmName.SHA256, 32);
    }
}
