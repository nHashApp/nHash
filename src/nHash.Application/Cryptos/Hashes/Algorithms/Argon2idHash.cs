using System.Security.Cryptography;
using Isopoh.Cryptography.Argon2;

namespace nHash.Application.Cryptos.Hashes.Algorithms;

public class Argon2idHash : IHash
{
    private readonly int _memoryKb;
    private readonly int _iterations;
    private readonly int _parallelism;
    private readonly int _hashLength;

    public Argon2idHash(int memoryKb = 65536, int iterations = 3, int parallelism = 4, int hashLength = 32)
    {
        _memoryKb = memoryKb;
        _iterations = iterations;
        _parallelism = parallelism;
        _hashLength = hashLength;
    }

    public byte[] ComputeHash(byte[] buffer)
    {
        var salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var config = new Argon2Config
        {
            Type = Argon2Type.HybridAddressing,
            Version = Argon2Version.Nineteen,
            Password = buffer,
            Salt = salt,
            Threads = _parallelism,
            Lanes = _parallelism,
            TimeCost = _iterations,
            MemoryCost = _memoryKb,
            HashLength = _hashLength
        };

        var argon2 = new Argon2(config);
        using (var hash = argon2.Hash())
        {
            var result = new byte[salt.Length + hash.Buffer.Length];
            Array.Copy(salt, 0, result, 0, salt.Length);
            Array.Copy(hash.Buffer, 0, result, salt.Length, hash.Buffer.Length);
            return result;
        }
    }
}
