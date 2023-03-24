namespace nHash.Application.Hashes.Algorithms;

internal class Adler32Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
    {
        uint a = 1, b = 0;
        for (var i = 0; i < buffer.Length; i++)
        {
            a = (a + buffer[i]) % 65521;
            b = (b + a) % 65521;
        }
        var res= (b << 16) | a;

        //Console.WriteLine(BitConverter.ToString(res));
        var hash= BitConverter.GetBytes(res);
        Array.Reverse(hash);
        return hash;
    }
}