namespace nHash.Application.Cryptos.Hashes;

public interface IHash
{
    public byte[] ComputeHash(byte[] buffer);
}