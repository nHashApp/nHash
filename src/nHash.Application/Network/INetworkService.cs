namespace nHash.Application.Network;

public interface INetworkService
{
    Task<string> GetIpAddressAsync(bool external);
    Task<string> ResolveDnsAsync(string hostname, string recordType);
    Task<string> ScanPortAsync(string host, int port);
    Task<string> QueryWhoisAsync(string domain);
}
