using nHash.Application.Network.Models;

namespace nHash.Application.Network;

public interface INetworkService
{
    Task<string> GetIpAddressAsync(bool external);
    Task<DnsResolveResult> ResolveDnsAsync(string hostname, string recordType);
    Task<PortScanResult> ScanPortAsync(string host, int port);
    Task<WhoisResult> QueryWhoisAsync(string domain);
    Task<HttpPingResult> HttpPingAsync(string url, int timeoutSeconds);
    Task<SslInfoResult> GetSslInfoAsync(string hostname);
    CidrResult CalculateCidr(string cidrNotation);
    Task<MacLookupResult> LookupMacVendorAsync(string macAddress);
}

