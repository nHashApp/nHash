namespace nHash.Providers.Hashing;

public interface IHash
{
    public byte[] ComputeHash(byte[] buffer);
}