namespace nHash.Application.Hashes.Algorithms;

public class MurmurHash3Hash : IHash
{
    private const uint Seed = 0;

    public byte[] ComputeHash(byte[] buffer)
    {
        uint hash = Seed;
        int length = buffer.Length;
        int nBlocks = length / 4;

        for (int i = 0; i < nBlocks; i++)
        {
            int baseIdx = i * 4;
            uint k = (uint)(buffer[baseIdx] | (buffer[baseIdx + 1] << 8) | (buffer[baseIdx + 2] << 16) | (buffer[baseIdx + 3] << 24));

            k *= 0xcc9e2d51;
            k = (k << 15) | (k >> 17);
            k *= 0x1b873593;

            hash ^= k;
            hash = (hash << 13) | (hash >> 19);
            hash = (hash * 5) + 0xe6546b64;
        }

        uint k1 = 0;
        int tailIdx = nBlocks * 4;
        int remaining = length & 3;

        if (remaining >= 3) k1 ^= (uint)(buffer[tailIdx + 2] << 16);
        if (remaining >= 2) k1 ^= (uint)(buffer[tailIdx + 1] << 8);
        if (remaining >= 1)
        {
            k1 ^= buffer[tailIdx];
            k1 *= 0xcc9e2d51;
            k1 = (k1 << 15) | (k1 >> 17);
            k1 *= 0x1b873593;
            hash ^= k1;
        }

        hash ^= (uint)length;
        hash ^= hash >> 16;
        hash *= 0x85ebca6b;
        hash ^= hash >> 13;
        hash *= 0xc2b2ae35;
        hash ^= hash >> 16;

        return BitConverter.GetBytes(hash);
    }
}
