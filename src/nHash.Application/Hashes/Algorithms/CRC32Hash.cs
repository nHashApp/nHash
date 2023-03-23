namespace nHash.Application.Hashes.Algorithms;

public class CRC32Hash : IHash
{
    private readonly uint[] _checksumTable;
    private const uint Polynomial = 0xEDB88320;

    public CRC32Hash()
    {
        _checksumTable = new uint[0x100];

        for (uint index = 0; index < 0x100; ++index)
        {
            var item = index;
            for (int bit = 0; bit < 8; ++bit)
                item = ((item & 1) != 0) ? (Polynomial ^ (item >> 1)) : (item >> 1);
            _checksumTable[index] = item;
        }
    }

    public byte[] ComputeHash(byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        return ComputeHash(stream);
    }

    private byte[] ComputeHash(Stream stream)
    {
        var result = 0xFFFFFFFF;

        int current;
        while ((current = stream.ReadByte()) != -1)
        {
            result = _checksumTable[(result & 0xFF) ^ (byte)current] ^ (result >> 8);
        }

        var hash = BitConverter.GetBytes(~result);
        Array.Reverse(hash);
        return hash;
    }
}