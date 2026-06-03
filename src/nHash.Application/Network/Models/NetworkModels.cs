using System;
using System.Collections.Generic;

namespace nHash.Application.Network.Models;

public class DnsRecordDetail
{
    public string Value { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
}

public class DnsResolveResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
    public List<string> Aliases { get; set; } = new();
    public List<DnsRecordDetail> Records { get; set; } = new();
    
    // For single query records (A/AAAA)
    public bool IsSingleTypeQuery { get; set; }
    public string RecordType { get; set; } = string.Empty;
    public List<string> SingleTypeValues { get; set; } = new();
}

public class PortScanResult
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool IsOpen { get; set; }
    public string StatusDetails { get; set; } = string.Empty;
}

public class WhoisResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string RawWhoisText { get; set; } = string.Empty;
}

public class HttpPingResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string StatusCode { get; set; } = string.Empty;
    public int StatusCodeNumber { get; set; }
    public long ElapsedMs { get; set; }
    public long? ContentLengthBytes { get; set; }
    public string Server { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}

public class SslInfoResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Hostname { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public string Thumbprint { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public bool IsExpired { get; set; }
    public int DaysUntilExpiry { get; set; }
}

public class CidrResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string CidrNotation { get; set; } = string.Empty;
    public string NetworkAddress { get; set; } = string.Empty;
    public string BroadcastAddress { get; set; } = string.Empty;
    public string SubnetMask { get; set; } = string.Empty;
    public string FirstUsableIp { get; set; } = string.Empty;
    public string LastUsableIp { get; set; } = string.Empty;
    public uint TotalHosts { get; set; }
    public uint UsableHosts { get; set; }
}

public class MacLookupResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
}
