using Isopoh.Cryptography.Argon2;

namespace nHash.Application.Hashes.Algorithms;

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
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var config = new Argon2Config
        {
            Type = Argon2Type.DataIndependentAddressing,
            Version = Argon2Version.Nineteen,
            Password = buffer,
            Salt = salt,
            Threads = _parallelism,
            Iterations = _iterations,
            Memory = _memoryKb,
            HashLength = _hashLength
        };

        var hash = Argon2.Hash(config);
        
        var result = new byte[salt.Length + hash.Length];
        Array.Copy(salt, 0, result, 0, salt.Length);
        Array.Copy(hash, 0, result, salt.Length, hash.Length);
        
        return result;
    }

    public bool VerifyHash(byte[] buffer, byte[] storedHashWithSalt)
    {
        if (storedHashWithSalt.Length < 16)
        {
            return false;
        }

        var salt = new byte[16];
        Array.Copy(storedHashWithSalt, 0, salt, 0, 16);

        var expectedHash = new byte[storedHashWithSalt.Length - 16];
        Array.Copy(storedHashWithSalt, 16, expectedHash, 0, expectedHash.Length);

        var config = new Argon2Config
        {
            Type = Argon2Type.DataIndependentAddressing,
            Version = Argon2Version.Nineteen,
            Password = buffer,
            Salt = salt,
            Threads = _parallelism,
            Iterations = _iterations,
            Memory = _memoryKb,
            HashLength = expectedHash.Length
        };

        var computedHash = Argon2.Hash(config);

        if (computedHash.Length != expectedHash.Length)
        {
            return false;
        }

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != expectedHash[i])
            {
                return false;
            }
        }

        return true;
    }
}
