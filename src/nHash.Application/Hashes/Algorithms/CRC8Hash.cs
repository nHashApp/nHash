namespace nHash.Application.Hashes.Algorithms;

internal class CRC8Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
    {
        byte crc = 0;

        for (var i = 0; i < buffer.Length; i++)
        {
            crc ^= buffer[i];

            for (var j = 0; j < 8; j++)
            {
                if ((crc & 0x80) != 0)
                {
                    crc = (byte)((crc << 1) ^ 0x07);
                }
                else
                {
                    crc <<= 1;
                }
            }
        }

        return new[] { crc };
    }
}