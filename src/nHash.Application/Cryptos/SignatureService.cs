using System.Security.Cryptography;
using System.Text;

namespace nHash.Application.Cryptos;

public class SignatureService : ISignatureService
{
    public string GenerateKeyPair(int keySize)
    {
        try
        {
            using var rsa = RSA.Create(keySize);
            var privateKey = rsa.ExportRSAPrivateKeyPem();
            var publicKey = rsa.ExportRSAPublicKeyPem();

            var sb = new StringBuilder();
            sb.AppendLine($"Key Size:   {keySize} bits");
            sb.AppendLine();
            sb.AppendLine("--- PRIVATE KEY ---");
            sb.AppendLine(privateKey);
            sb.AppendLine();
            sb.AppendLine("--- PUBLIC KEY ---");
            sb.AppendLine(publicKey);
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Error generating key pair: {ex.Message}";
        }
    }

    public string Sign(string data, string privateKeyPem)
    {
        if (string.IsNullOrWhiteSpace(data))
            return "Error: Data cannot be empty.";
        if (string.IsNullOrWhiteSpace(privateKeyPem))
            return "Error: Private key cannot be empty.";

        try
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem.AsSpan());

            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signature = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var signatureBase64 = Convert.ToBase64String(signature);

            var sb = new StringBuilder();
            sb.AppendLine($"Algorithm:  RSA-SHA256 (PKCS#1 v1.5)");
            sb.AppendLine($"Key Size:   {rsa.KeySize} bits");
            sb.AppendLine($"Data:       {data}");
            sb.AppendLine($"Signature:  {signatureBase64}");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Error signing data: {ex.Message}";
        }
    }

    public string Verify(string data, string signature, string publicKeyPem)
    {
        if (string.IsNullOrWhiteSpace(data))
            return "Error: Data cannot be empty.";
        if (string.IsNullOrWhiteSpace(signature))
            return "Error: Signature cannot be empty.";
        if (string.IsNullOrWhiteSpace(publicKeyPem))
            return "Error: Public key cannot be empty.";

        try
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem.AsSpan());

            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signatureBytes = Convert.FromBase64String(signature);
            bool valid = rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            var sb = new StringBuilder();
            sb.AppendLine($"Algorithm:  RSA-SHA256 (PKCS#1 v1.5)");
            sb.AppendLine($"Data:       {data}");
            sb.AppendLine($"Valid:       {valid}");
            sb.AppendLine($"Result:     {(valid ? "✓ Signature is VALID" : "✗ Signature is INVALID")}");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Error verifying signature: {ex.Message}";
        }
    }
}
