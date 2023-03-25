namespace nHash.Application.Hashes.Algorithms;

internal class Fletcher16Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
    {
        ushort sum1 = 0;
        ushort sum2 = 0;

        for (var i = 0; i < buffer.Length; i++)
        {
            sum1 = (ushort)((sum1 + buffer[i]) % 255);
            sum2 = (ushort)((sum2 + sum1) % 255);
        }

        var hash= BitConverter.GetBytes(((ushort)((sum2 << 8) | sum1)));
        Array.Reverse(hash);
        return hash;
    }
}