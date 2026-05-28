namespace nHash.Application.Cryptos.Hashes;

public interface IHmacService
{
    string Calculate(string text, string key, string algorithm);
}
