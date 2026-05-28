using System;

namespace nHash.Application.Hashes.Algorithms;

public class Crc16Hash : IHash
{
    public byte[] ComputeHash(byte[] buffer)
    {
        ushort crc = 0xFFFF;
        foreach (byte b in buffer)
        {
            crc ^= b;
            for (int i = 0; i < 8; i++)
            {
                if ((crc & 1) != 0)
                {
                    crc = (ushort)((crc >> 1) ^ 0xA001); // Modbus polynomial
                }
                else
                {
                    crc >>= 1;
                }
            }
        }
        return BitConverter.GetBytes(crc);
    }
}
