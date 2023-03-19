namespace nHash.Application.Hashes;

public interface IHash
{
    public byte[] ComputeHash(byte[] buffer);
}