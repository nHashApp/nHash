using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using nHash.Application.Network.Models;

namespace nHash.Application.Network;

public class NetworkService : INetworkService
{
    private static readonly HttpClient HttpClientInstance = new() { Timeout = TimeSpan.FromSeconds(5) };

    public async Task<string> GetIpAddressAsync(bool external)
    {
        if (!external)
        {
            try
            {
                var host = await Dns.GetHostEntryAsync(Dns.GetHostName());
                var localIps = host.AddressList
                    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                    .Select(ip => ip.ToString());
                return string.Join(Environment.NewLine, localIps);
            }
            catch (Exception ex)
            {
                return $"Error resolving internal IP: {ex.Message}";
            }
        }

        try
        {
            var response = await HttpClientInstance.GetStringAsync("https://api.ipify.org");
            var ip = response.Trim();
            try
            {
                var geoResponse = await HttpClientInstance.GetStringAsync($"https://ipapi.co/{ip}/country_name/");
                if (!string.IsNullOrWhiteSpace(geoResponse) && !geoResponse.Contains("error"))
                {
                    return $"{ip} ({geoResponse.Trim()})";
                }
            }
            catch
            {
                // Ignore geo resolution failures, just return the IP
            }
            return ip;
        }
        catch (Exception ex)
        {
            return $"Error resolving external IP: {ex.Message}";
        }
    }

    public async Task<DnsResolveResult> ResolveDnsAsync(string hostname, string recordType)
    {
        var result = new DnsResolveResult();
        var type = recordType.ToUpperInvariant().Trim();
        try
        {
            if (type is "A" or "AAAA")
            {
                var addresses = await Dns.GetHostAddressesAsync(hostname);
                var family = type == "A" ? AddressFamily.InterNetwork : AddressFamily.InterNetworkV6;
                var filtered = addresses
                    .Where(ip => ip.AddressFamily == family)
                    .Select(ip => ip.ToString())
                    .ToList();

                result.IsSingleTypeQuery = true;
                result.RecordType = type;
                result.SingleTypeValues = filtered;
                result.Success = true;
                return result;
            }

            var entry = await Dns.GetHostEntryAsync(hostname);
            result.HostName = entry.HostName;
            result.Aliases = entry.Aliases.ToList();
            foreach (var ip in entry.AddressList)
            {
                result.Records.Add(new DnsRecordDetail
                {
                    Value = ip.ToString(),
                    Family = ip.AddressFamily.ToString()
                });
            }
            result.IsSingleTypeQuery = false;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"DNS Query Error: {ex.Message}";
            return result;
        }
    }

    public async Task<PortScanResult> ScanPortAsync(string host, int port)
    {
        var result = new PortScanResult { Host = host, Port = port };
        try
        {
            using var tcpClient = new TcpClient();
            var connectTask = tcpClient.ConnectAsync(host, port);
            var delayTask = Task.Delay(1500);

            var completedTask = await Task.WhenAny(connectTask, delayTask);
            if (completedTask == connectTask)
            {
                await connectTask;
                result.IsOpen = true;
                result.StatusDetails = "OPEN";
            }
            else
            {
                result.IsOpen = false;
                result.StatusDetails = "CLOSED (Timeout)";
            }
            return result;
        }
        catch (Exception ex)
        {
            result.IsOpen = false;
            result.StatusDetails = $"CLOSED ({ex.Message})";
            return result;
        }
    }

    public async Task<WhoisResult> QueryWhoisAsync(string domain)
    {
        var result = new WhoisResult();
        if (string.IsNullOrWhiteSpace(domain))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Domain cannot be empty.";
            return result;
        }
        domain = domain.Trim().ToLowerInvariant();

        var tld = domain.Split('.').Last();
        var whoisServer = tld switch
        {
            "com" => "whois.verisign-grs.com",
            "net" => "whois.verisign-grs.com",
            "org" => "whois.pir.org",
            "info" => "whois.afilias.net",
            "ir" => "whois.nic.ir",
            "io" => "whois.nic.io",
            "co" => "whois.nic.co",
            "uk" => "whois.nic.uk",
            _ => "whois.iana.org"
        };

        try
        {
            using var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(whoisServer, 43);
            
            using var stream = tcpClient.GetStream();
            using var writer = new StreamWriter(stream, Encoding.ASCII);
            using var reader = new StreamReader(stream, Encoding.UTF8);

            await writer.WriteAsync(domain + "\r\n");
            await writer.FlushAsync();

            var sb = new StringBuilder();
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                sb.AppendLine(line);
            }

            result.Domain = domain;
            result.RawWhoisText = sb.ToString();
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"WHOIS Query Error: {ex.Message}";
            return result;
        }
    }

    public async Task<HttpPingResult> HttpPingAsync(string url, int timeoutSeconds)
    {
        var result = new HttpPingResult();
        if (string.IsNullOrWhiteSpace(url))
        {
            result.Success = false;
            result.ErrorMessage = "Error: URL cannot be empty.";
            return result;
        }
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && 
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            url = "https://" + url;
        }

        try
        {
            using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
            var sw = System.Diagnostics.Stopwatch.StartNew();
            
            using var client = new HttpClient();
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cts.Token);
            sw.Stop();

            result.Url = url;
            result.StatusCode = response.StatusCode.ToString();
            result.StatusCodeNumber = (int)response.StatusCode;
            result.ElapsedMs = sw.ElapsedMilliseconds;
            result.ContentLengthBytes = response.Content.Headers.ContentLength;
            if (response.Headers.Server.Any())
            {
                result.Server = string.Join(", ", response.Headers.Server);
            }
            if (response.Content.Headers.ContentType != null)
            {
                result.ContentType = response.Content.Headers.ContentType.ToString();
            }
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"HTTP Ping Error: {ex.Message}";
            return result;
        }
    }

    public async Task<SslInfoResult> GetSslInfoAsync(string hostname)
    {
        var result = new SslInfoResult();
        if (string.IsNullOrWhiteSpace(hostname))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Hostname cannot be empty.";
            return result;
        }
        hostname = hostname.Trim().Replace("https://", "").Replace("http://", "").Split('/').First();

        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(hostname, 443);

            using var sslStream = new System.Net.Security.SslStream(
                client.GetStream(),
                false,
                (sender, certificate, chain, sslPolicyErrors) => true
            );

            await sslStream.AuthenticateAsClientAsync(hostname);

            var cert = sslStream.RemoteCertificate as System.Security.Cryptography.X509Certificates.X509Certificate2;
            if (cert == null)
            {
                result.Success = false;
                result.ErrorMessage = "Error: Could not retrieve SSL certificate.";
                return result;
            }

            result.Hostname = hostname;
            result.Subject = cert.Subject;
            result.Issuer = cert.Issuer;
            result.ValidFrom = cert.NotBefore;
            result.ValidTo = cert.NotAfter;
            result.Thumbprint = cert.Thumbprint;
            result.SerialNumber = cert.SerialNumber;
            result.IsExpired = DateTime.Now < cert.NotBefore || DateTime.Now > cert.NotAfter;
            result.DaysUntilExpiry = (cert.NotAfter - DateTime.Now).Days;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"SSL Certificate Error: {ex.Message}";
            return result;
        }
    }

    public CidrResult CalculateCidr(string cidrNotation)
    {
        var result = new CidrResult();
        if (string.IsNullOrWhiteSpace(cidrNotation))
        {
            result.Success = false;
            result.ErrorMessage = "Error: CIDR notation cannot be empty.";
            return result;
        }
        var parts = cidrNotation.Trim().Split('/');
        if (parts.Length != 2)
        {
            result.Success = false;
            result.ErrorMessage = "Error: Invalid CIDR format. Expected format: 192.168.1.0/24";
            return result;
        }

        if (!IPAddress.TryParse(parts[0], out var ip) || ip.AddressFamily != AddressFamily.InterNetwork)
        {
            result.Success = false;
            result.ErrorMessage = "Error: Invalid IPv4 address.";
            return result;
        }

        if (!int.TryParse(parts[1], out var maskLen) || maskLen < 0 || maskLen > 32)
        {
            result.Success = false;
            result.ErrorMessage = "Error: Invalid mask length. Must be between 0 and 32.";
            return result;
        }

        byte[] bytes = ip.GetAddressBytes();
        uint ipVal = ((uint)bytes[0] << 24) | ((uint)bytes[1] << 16) | ((uint)bytes[2] << 8) | bytes[3];

        uint mask = maskLen == 0 ? 0 : 0xFFFFFFFF << (32 - maskLen);
        uint network = ipVal & mask;
        uint wildcard = ~mask;
        uint broadcast = network | wildcard;

        uint firstUsable = network + 1;
        uint lastUsable = broadcast - 1;

        if (maskLen == 31)
        {
            firstUsable = network;
            lastUsable = broadcast;
        }
        else if (maskLen == 32)
        {
            firstUsable = network;
            lastUsable = network;
        }

        uint totalHosts = (uint)System.Math.Pow(2, 32 - maskLen);
        uint usableHosts = maskLen >= 31 ? totalHosts : totalHosts - 2;

        static string UintToIp(uint val)
        {
            return $"{(val >> 24) & 0xFF}.{(val >> 16) & 0xFF}.{(val >> 8) & 0xFF}.{val & 0xFF}";
        }

        result.CidrNotation = cidrNotation;
        result.NetworkAddress = UintToIp(network);
        result.BroadcastAddress = UintToIp(broadcast);
        result.SubnetMask = UintToIp(mask);
        result.FirstUsableIp = usableHosts > 0 ? UintToIp(firstUsable) : "N/A";
        result.LastUsableIp = usableHosts > 0 ? UintToIp(lastUsable) : "N/A";
        result.TotalHosts = totalHosts;
        result.UsableHosts = usableHosts;
        result.Success = true;
        return result;
    }

    public async Task<MacLookupResult> LookupMacVendorAsync(string macAddress)
    {
        var result = new MacLookupResult();
        if (string.IsNullOrWhiteSpace(macAddress))
        {
            result.Success = false;
            result.ErrorMessage = "Error: MAC address cannot be empty.";
            return result;
        }
        
        var cleanMac = new string(macAddress.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
        if (cleanMac.Length < 6)
        {
            result.Success = false;
            result.ErrorMessage = "Error: Invalid MAC address. Must be at least 6 hex characters.";
            return result;
        }

        try
        {
            var url = $"https://api.maclookup.app/v2/macs/{cleanMac}";
            var response = await HttpClientInstance.GetStringAsync(url);
            
            using var doc = JsonDocument.Parse(response);
            var root = doc.RootElement;
            result.MacAddress = macAddress;
            
            if (root.TryGetProperty("found", out var foundProp) && foundProp.GetBoolean())
            {
                if (root.TryGetProperty("company", out var companyProp))
                {
                    var vendor = companyProp.GetString();
                    if (!string.IsNullOrWhiteSpace(vendor))
                    {
                        result.VendorName = vendor;
                        result.Success = true;
                        return result;
                    }
                }
            }
            result.VendorName = "Unknown";
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"MAC Vendor Lookup Error: {ex.Message}";
            return result;
        }
    }
}
