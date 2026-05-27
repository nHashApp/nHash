using System.Net;
using System.Net.Sockets;
using System.Text;

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

    public async Task<string> ResolveDnsAsync(string hostname, string recordType)
    {
        var type = recordType.ToUpperInvariant().Trim();
        try
        {
            if (type is "A" or "AAAA")
            {
                var addresses = await Dns.GetHostAddressesAsync(hostname);
                var family = type == "A" ? AddressFamily.InterNetwork : AddressFamily.InterNetworkV6;
                var filtered = addresses
                    .Where(ip => ip.AddressFamily == family)
                    .Select(ip => ip.ToString());

                return filtered.Any() 
                    ? string.Join(Environment.NewLine, filtered) 
                    : $"No {type} records found for {hostname}.";
            }

            var entry = await Dns.GetHostEntryAsync(hostname);
            var sb = new StringBuilder();
            sb.AppendLine($"Host Name: {entry.HostName}");
            if (entry.Aliases.Any())
            {
                sb.AppendLine($"Aliases: {string.Join(", ", entry.Aliases)}");
            }
            sb.AppendLine("IP Addresses:");
            foreach (var ip in entry.AddressList)
            {
                sb.AppendLine($"- {ip} ({ip.AddressFamily})");
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"DNS Query Error: {ex.Message}";
        }
    }

    public async Task<string> ScanPortAsync(string host, int port)
    {
        try
        {
            using var tcpClient = new TcpClient();
            var connectTask = tcpClient.ConnectAsync(host, port);
            var delayTask = Task.Delay(1500);

            var completedTask = await Task.WhenAny(connectTask, delayTask);
            if (completedTask == connectTask)
            {
                await connectTask;
                return $"Port {port} on {host} is OPEN.";
            }
            else
            {
                return $"Port {port} on {host} is CLOSED (Timeout).";
            }
        }
        catch (Exception ex)
        {
            return $"Port {port} on {host} is CLOSED ({ex.Message}).";
        }
    }

    public async Task<string> QueryWhoisAsync(string domain)
    {
        if (string.IsNullOrWhiteSpace(domain)) return "Error: Domain cannot be empty.";
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

            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"WHOIS Query Error: {ex.Message}";
        }
    }

    public async Task<string> HttpPingAsync(string url, int timeoutSeconds)
    {
        if (string.IsNullOrWhiteSpace(url)) return "Error: URL cannot be empty.";
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

            var sb = new StringBuilder();
            sb.AppendLine($"HTTP Ping to: {url}");
            sb.AppendLine($"Status: {response.StatusCode} ({(int)response.StatusCode})");
            sb.AppendLine($"Latency: {sw.ElapsedMilliseconds} ms");
            sb.AppendLine($"Content Length: {response.Content.Headers.ContentLength?.ToString() ?? "Unknown"} bytes");
            if (response.Headers.Server.Any())
            {
                sb.AppendLine($"Server: {string.Join(", ", response.Headers.Server)}");
            }
            if (response.Content.Headers.ContentType != null)
            {
                sb.AppendLine($"Content Type: {response.Content.Headers.ContentType}");
            }
            return sb.ToString().TrimEnd();
        }
        catch (Exception ex)
        {
            return $"HTTP Ping Error: {ex.Message}";
        }
    }

    public async Task<string> GetSslInfoAsync(string hostname)
    {
        if (string.IsNullOrWhiteSpace(hostname)) return "Error: Hostname cannot be empty.";
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
                return "Error: Could not retrieve SSL certificate.";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"SSL/TLS Certificate Info for: {hostname}");
            sb.AppendLine($"Subject: {cert.Subject}");
            sb.AppendLine($"Issuer: {cert.Issuer}");
            sb.AppendLine($"Valid From: {cert.NotBefore}");
            sb.AppendLine($"Valid To: {cert.NotAfter}");
            sb.AppendLine($"Thumbprint: {cert.Thumbprint}");
            
            var serial = cert.SerialNumber;
            sb.AppendLine($"Serial Number: {serial}");

            var isExpired = DateTime.Now < cert.NotBefore || DateTime.Now > cert.NotAfter;
            sb.AppendLine($"Is Expired: {(isExpired ? "Yes" : "No")}");
            
            var daysLeft = (cert.NotAfter - DateTime.Now).Days;
            sb.AppendLine($"Days until expiry: {daysLeft}");

            return sb.ToString().TrimEnd();
        }
        catch (Exception ex)
        {
            return $"SSL Certificate Error: {ex.Message}";
        }
    }

    public string CalculateCidr(string cidrNotation)
    {
        if (string.IsNullOrWhiteSpace(cidrNotation)) return "Error: CIDR notation cannot be empty.";
        var parts = cidrNotation.Trim().Split('/');
        if (parts.Length != 2) return "Error: Invalid CIDR format. Expected format: 192.168.1.0/24";

        if (!IPAddress.TryParse(parts[0], out var ip) || ip.AddressFamily != AddressFamily.InterNetwork)
        {
            return "Error: Invalid IPv4 address.";
        }

        if (!int.TryParse(parts[1], out var maskLen) || maskLen < 0 || maskLen > 32)
        {
            return "Error: Invalid mask length. Must be between 0 and 32.";
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

        var sb = new StringBuilder();
        sb.AppendLine($"CIDR Input: {cidrNotation}");
        sb.AppendLine($"Network Address: {UintToIp(network)}");
        sb.AppendLine($"Broadcast Address: {UintToIp(broadcast)}");
        sb.AppendLine($"Subnet Mask: {UintToIp(mask)}");
        sb.AppendLine($"First Usable IP: {(usableHosts > 0 ? UintToIp(firstUsable) : "N/A")}");
        sb.AppendLine($"Last Usable IP: {(usableHosts > 0 ? UintToIp(lastUsable) : "N/A")}");
        sb.AppendLine($"Total Hosts: {totalHosts:N0}");
        sb.AppendLine($"Usable Hosts: {usableHosts:N0}");

        return sb.ToString().TrimEnd();
    }

    public async Task<string> LookupMacVendorAsync(string macAddress)
    {
        if (string.IsNullOrWhiteSpace(macAddress)) return "Error: MAC address cannot be empty.";
        
        var cleanMac = new string(macAddress.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
        if (cleanMac.Length < 6) return "Error: Invalid MAC address. Must be at least 6 hex characters.";

        try
        {
            var url = $"https://api.maclookup.app/v2/macs/{cleanMac}";
            var response = await HttpClientInstance.GetStringAsync(url);
            
            using var doc = System.Text.Json.JsonDocument.Parse(response);
            var root = doc.RootElement;
            if (root.TryGetProperty("found", out var foundProp) && foundProp.GetBoolean())
            {
                if (root.TryGetProperty("company", out var companyProp))
                {
                    var vendor = companyProp.GetString();
                    if (!string.IsNullOrWhiteSpace(vendor))
                    {
                        return $"MAC: {macAddress}\nVendor: {vendor}";
                    }
                }
            }
            return $"MAC: {macAddress}\nVendor: Unknown";
        }
        catch (Exception ex)
        {
            return $"MAC Vendor Lookup Error: {ex.Message}";
        }
    }
}

