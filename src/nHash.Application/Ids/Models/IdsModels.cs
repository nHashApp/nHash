namespace nHash.Application.Ids.Models;

public class TotpGenerateResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Digits { get; set; }
    public int PeriodSeconds { get; set; }
    public int RemainingSeconds { get; set; }
}

public class TotpRemainingResult
{
    public int RemainingSeconds { get; set; }
    public int PeriodSeconds { get; set; }
}

public class UuidInspectResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public Guid ParsedGuid { get; set; }
    public int Version { get; set; }
    public string Variant { get; set; } = string.Empty;
    public DateTime? Timestamp { get; set; }
    public string TimestampDescription { get; set; } = string.Empty;
    public long? ClockSequence { get; set; }
    public long? UnixMs { get; set; }
    public bool? IsRandom { get; set; }
    public byte[] RfcBytes { get; set; } = Array.Empty<byte>();
}
