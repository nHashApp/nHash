using System.Security.Cryptography;
using Blake2Fast;

namespace nHash.Application.Hashes.Algorithms;

public class Argon2idHash : IHash
{
    private static readonly byte[] DefaultSalt = "nHashSalt1234567"u8.ToArray();

    public byte[] ComputeHash(byte[] buffer)
    {
        uint lanes = 1;
        uint memoryKb = 64; 
        uint iterations = 2;
        uint tagLength = 32;

        uint blockCount = memoryKb;
        var blocks = new ulong[blockCount][];
        for (int i = 0; i < blockCount; i++)
        {
            blocks[i] = new ulong[128];
        }
        
        var hasher = Blake2b.CreateIncrementalHasher(64);
        hasher.Update(BitConverter.GetBytes(lanes));
        hasher.Update(BitConverter.GetBytes(tagLength));
        hasher.Update(BitConverter.GetBytes(memoryKb));
        hasher.Update(BitConverter.GetBytes(iterations));
        hasher.Update(buffer);
        hasher.Update(DefaultSalt);
        var h0 = hasher.Finish();

        for (uint i = 0; i < 2; i++)
        {
            var temp = new byte[1024];
            var blockHasher = Blake2b.CreateIncrementalHasher(64);
            blockHasher.Update(h0);
            blockHasher.Update(BitConverter.GetBytes(i)); 
            blockHasher.Update(BitConverter.GetBytes((uint)0)); 
            var hash = blockHasher.Finish();
            
            for (int offset = 0; offset < 1024; offset += 64)
            {
                Array.Copy(hash, 0, temp, offset, 64);
                hash = Blake2b.ComputeHash(64, hash);
            }
            
            for (int j = 0; j < 128; j++)
            {
                blocks[i][j] = BitConverter.ToUInt64(temp, j * 8);
            }
        }

        for (uint i = 2; i < blockCount; i++)
        {
            uint refBlock = (i - 1) % i; 
            MixBlocks(blocks[i - 1], blocks[refBlock], blocks[i]);
        }

        var finalBlock = blocks[blockCount - 1];
        var finalBytes = new byte[1024];
        for (int j = 0; j < 128; j++)
        {
            Array.Copy(BitConverter.GetBytes(finalBlock[j]), 0, finalBytes, j * 8, 8);
        }

        var result = Blake2b.ComputeHash((int)tagLength, finalBytes);
        return result;
    }

    private static void MixBlocks(ulong[] prev, ulong[] refBlock, ulong[] next)
    {
        for (int j = 0; j < 128; j++)
        {
            next[j] = prev[j] ^ refBlock[j];
        }
        for (int i = 0; i < 8; i++)
        {
            MixRound(next, i * 16);
        }
    }

    private static void MixRound(ulong[] state, int offset)
    {
        state[offset + 0] += state[offset + 4];
        state[offset + 12] ^= state[offset + 0];
        state[offset + 12] = (state[offset + 12] << 32) | (state[offset + 12] >> 32);
        
        state[offset + 8] += state[offset + 12];
        state[offset + 4] ^= state[offset + 8];
        state[offset + 4] = (state[offset + 4] << 24) | (state[offset + 4] >> 40);
    }
}
