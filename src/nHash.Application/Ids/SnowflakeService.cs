using System;

namespace nHash.Application.Ids;

public class SnowflakeService : ISnowflakeService
{
    private static readonly DateTime Epoch = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private readonly object _lock = new();

    private long _lastTimestamp = -1;
    private long _sequence = 0;

    public long Generate(int workerId)
    {
        lock (_lock)
        {
            var timestamp = GetTimestamp();

            if (timestamp < _lastTimestamp)
            {
                throw new InvalidOperationException("Clock moved backwards.");
            }

            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & 4095;
                if (_sequence == 0)
                {
                    timestamp = WaitNextMillis(_lastTimestamp);
                }
            }
            else
            {
                _sequence = 0;
            }

            _lastTimestamp = timestamp;

            return (timestamp << 22) | ((long)(workerId & 1023) << 12) | _sequence;
        }
    }

    private static long GetTimestamp()
    {
        return (long)(DateTime.UtcNow - Epoch).TotalMilliseconds;
    }

    private static long WaitNextMillis(long lastTimestamp)
    {
        var timestamp = GetTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetTimestamp();
        }
        return timestamp;
    }
}
