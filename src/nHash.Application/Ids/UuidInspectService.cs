using System.Text;

namespace nHash.Application.Ids;

public class UuidInspectService : IUuidInspectService
{
    public string Inspect(string uuid)
    {
        if (string.IsNullOrWhiteSpace(uuid))
            return "Error: UUID cannot be empty.";

        try
        {
            var guid = Guid.Parse(uuid);
            var bytes = guid.ToByteArray();

            // Guid.ToByteArray() returns bytes in a mixed-endian order.
            // We need the actual RFC 4122 byte layout:
            // time_low (4 bytes) + time_mid (2 bytes) + time_hi_and_version (2 bytes) + clock_seq (2 bytes) + node (6 bytes)
            // Guid stores first 3 fields in little-endian, so we need to reverse them.
            var rfcBytes = new byte[16];
            // time_low: bytes[3..0]
            rfcBytes[0] = bytes[3];
            rfcBytes[1] = bytes[2];
            rfcBytes[2] = bytes[1];
            rfcBytes[3] = bytes[0];
            // time_mid: bytes[5..4]
            rfcBytes[4] = bytes[5];
            rfcBytes[5] = bytes[4];
            // time_hi_and_version: bytes[7..6]
            rfcBytes[6] = bytes[7];
            rfcBytes[7] = bytes[6];
            // clock_seq and node: bytes[8..15] (big-endian already)
            for (int i = 8; i < 16; i++) rfcBytes[i] = bytes[i];

            int version = (rfcBytes[6] & 0xF0) >> 4;
            int variantBits = rfcBytes[8] & 0xC0;
            string variant = variantBits switch
            {
                0x80 => "RFC 4122 (variant 1)",
                0xC0 => "Microsoft (variant 2)",
                0x00 => "NCS backward compatibility",
                _ => $"Reserved (0x{variantBits:X2})"
            };

            var sb = new StringBuilder();
            sb.AppendLine($"UUID:     {guid}");
            sb.AppendLine($"Version:  {version}");
            sb.AppendLine($"Variant:  {variant}");

            // Version-specific info
            switch (version)
            {
                case 1:
                {
                    // 60-bit timestamp: time_low (32 bits) | time_mid (16 bits) | time_hi (12 bits)
                    long timeLow  = ((long)rfcBytes[0] << 24) | ((long)rfcBytes[1] << 16) | ((long)rfcBytes[2] << 8) | rfcBytes[3];
                    long timeMid  = ((long)rfcBytes[4] << 8)  | rfcBytes[5];
                    long timeHigh = ((long)(rfcBytes[6] & 0x0F) << 8) | rfcBytes[7];
                    long timestamp60 = (timeHigh << 48) | (timeMid << 32) | timeLow;

                    // 100-nanosecond intervals since Oct 15, 1582
                    var gregorianEpoch = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);
                    var ticks = gregorianEpoch.Ticks + timestamp60;
                    var dt = new DateTime(ticks, DateTimeKind.Utc);
                    sb.AppendLine($"Timestamp:{dt:yyyy-MM-dd HH:mm:ss.fffffff} UTC (v1 time-based)");

                    long clockSeq = ((rfcBytes[8] & 0x3F) << 8) | rfcBytes[9];
                    sb.AppendLine($"Clock Seq: {clockSeq}");
                    break;
                }
                case 4:
                    sb.AppendLine("Timestamp: N/A (random)");
                    sb.AppendLine("Random:    True (randomly generated UUID)");
                    break;
                case 7:
                {
                    // First 48 bits = Unix milliseconds timestamp
                    long ms = ((long)rfcBytes[0] << 40) | ((long)rfcBytes[1] << 32) | ((long)rfcBytes[2] << 24) |
                              ((long)rfcBytes[3] << 16) | ((long)rfcBytes[4] << 8)  | rfcBytes[5];
                    var dt = DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
                    sb.AppendLine($"Timestamp: {dt:yyyy-MM-dd HH:mm:ss.fff} UTC (v7 unix-ms)");
                    sb.AppendLine($"Unix ms:   {ms}");
                    break;
                }
                default:
                    sb.AppendLine($"Timestamp: N/A (v{version})");
                    break;
            }

            // Hex bytes
            sb.Append("Bytes:    ");
            for (int i = 0; i < rfcBytes.Length; i++)
            {
                if (i > 0) sb.Append(' ');
                sb.Append(rfcBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Error parsing UUID: {ex.Message}";
        }
    }
}
