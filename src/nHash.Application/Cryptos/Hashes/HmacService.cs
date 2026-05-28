using System.Security.Cryptography;
using System.Text;

namespace nHash.Application.Cryptos.Hashes;

public class HmacService : IHmacService
{
    public string Calculate(string text, string key, string algorithm)
    {
        var textBytes = Encoding.UTF8.GetBytes(text);
        var keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] hashBytes;

        switch (algorithm.ToLowerInvariant().Trim().Replace("-", ""))
        {
            case "md5":
                using (var hmac = new HMACMD5(keyBytes)) hashBytes = hmac.ComputeHash(textBytes);
                break;
            case "sha1":
                using (var hmac = new HMACSHA1(keyBytes)) hashBytes = hmac.ComputeHash(textBytes);
                break;
            case "sha256":
                using (var hmac = new HMACSHA256(keyBytes)) hashBytes = hmac.ComputeHash(textBytes);
                break;
            case "sha512":
                using (var hmac = new HMACSHA512(keyBytes)) hashBytes = hmac.ComputeHash(textBytes);
                break;
            default:
                using (var hmac = new HMACSHA256(keyBytes)) hashBytes = hmac.ComputeHash(textBytes);
                break;
        }

        return Convert.ToHexString(hashBytes);
    }
}
