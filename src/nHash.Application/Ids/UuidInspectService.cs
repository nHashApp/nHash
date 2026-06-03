using System;
using nHash.Application.Ids.Models;

namespace nHash.Application.Ids;

public class UuidInspectService : IUuidInspectService
{
    public UuidInspectResult Inspect(string uuid)
    {
        var result = new UuidInspectResult();
        if (string.IsNullOrWhiteSpace(uuid))
        {
            result.Success = false;
            result.ErrorMessage = "Error: UUID cannot be empty.";
            return result;
        }

        try
        {
            var guid = Guid.Parse(uuid);
            var bytes = guid.ToByteArray();

            var rfcBytes = new byte[16];
            rfcBytes[0] = bytes[3];
            rfcBytes[1] = bytes[2];
            rfcBytes[2] = bytes[1];
            rfcBytes[3] = bytes[0];
            rfcBytes[4] = bytes[5];
            rfcBytes[5] = bytes[4];
            rfcBytes[6] = bytes[7];
            rfcBytes[7] = bytes[6];
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

            result.ParsedGuid = guid;
            result.Version = version;
            result.Variant = variant;
            result.RfcBytes = rfcBytes;

            // Version-specific info
            switch (version)
            {
                case 1:
                {
                    long timeLow  = ((long)rfcBytes[0] << 24) | ((long)rfcBytes[1] << 16) | ((long)rfcBytes[2] << 8) | rfcBytes[3];
                    long timeMid  = ((long)rfcBytes[4] << 8)  | rfcBytes[5];
                    long timeHigh = ((long)(rfcBytes[6] & 0x0F) << 8) | rfcBytes[7];
                    long timestamp60 = (timeHigh << 48) | (timeMid << 32) | timeLow;

                    var gregorianEpoch = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);
                    var ticks = gregorianEpoch.Ticks + timestamp60;
                    result.Timestamp = new DateTime(ticks, DateTimeKind.Utc);
                    result.TimestampDescription = "UTC (v1 time-based)";

                    long clockSeq = ((rfcBytes[8] & 0x3F) << 8) | rfcBytes[9];
                    result.ClockSequence = clockSeq;
                    break;
                }
                case 4:
                    result.TimestampDescription = "N/A (random)";
                    result.IsRandom = true;
                    break;
                case 7:
                {
                    long ms = ((long)rfcBytes[0] << 40) | ((long)rfcBytes[1] << 32) | ((long)rfcBytes[2] << 24) |
                              ((long)rfcBytes[3] << 16) | ((long)rfcBytes[4] << 8)  | rfcBytes[5];
                    result.Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
                    result.TimestampDescription = "UTC (v7 unix-ms)";
                    result.UnixMs = ms;
                    break;
                }
                default:
                    result.TimestampDescription = $"N/A (v{version})";
                    break;
            }

            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error parsing UUID: {ex.Message}";
            return result;
        }
    }
}
