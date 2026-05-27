namespace nHash.Application.Cryptos;

public interface ISignatureService
{
    string GenerateKeyPair(int keySize);
    string Sign(string data, string privateKeyPem);
    string Verify(string data, string signature, string publicKeyPem);
}
