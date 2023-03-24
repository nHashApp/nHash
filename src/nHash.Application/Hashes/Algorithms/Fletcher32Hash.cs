namespace nHash.Application.Hashes.Algorithms;

internal class Fletcher32Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
    {
        uint sum1 = 0xFFFF;
        uint sum2 = 0xFFFF;

        for (var i = 0; i < buffer.Length; i++)
        {
            sum1 = (sum1 + buffer[i]) % 65535;
            sum2 = (sum2 + sum1) % 65535;
        }

        var hash= BitConverter.GetBytes(((sum2 << 16) | sum1));
        Array.Reverse(hash);
        return hash;
    }
}