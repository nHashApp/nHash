namespace nHash.Application.Cryptos;

public interface ICipherService
{
    string Encrypt(string plainText, string password, string algorithm);
    string Decrypt(string cipherText, string password, string algorithm);
}
