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
}
