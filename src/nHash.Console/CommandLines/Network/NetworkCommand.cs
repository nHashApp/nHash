using nHash.Application.Network;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Network;

public class NetworkCommand(INetworkService networkService, IOutputProvider outputProvider) : INetworkCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("net", "Network analysis and utility subcommands");
        command.Aliases.Add("network");

        command.Subcommands.Add(GetIpCommand());
        command.Subcommands.Add(GetDnsCommand());
        command.Subcommands.Add(GetPortCommand());
        command.Subcommands.Add(GetWhoisCommand());
        command.Subcommands.Add(GetPingCommand());
        command.Subcommands.Add(GetSslCommand());
        command.Subcommands.Add(GetCidrCommand());
        command.Subcommands.Add(GetMacCommand());

        return command;
    }

    private BaseCommand GetIpCommand()
    {
        var externalOption = new Option<bool>("--external", "-e") { Description = "Query external IP address (resolves geographic location)" };
        var cmd = new BaseCommand("ip", "Retrieve internal or external IP addresses");
        cmd.Options.Add(externalOption);

        cmd.SetAction(async parseResult =>
        {
            var ext = parseResult.GetValue(externalOption);
            var res = await networkService.GetIpAddressAsync(ext);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }

    private BaseCommand GetDnsCommand()
    {
        var hostArg = new Argument<string>("hostname") { Description = "Hostname/Domain to resolve" };
        var typeOption = new Option<string>("--type", "-t") { Description = "DNS query record type (A, AAAA, MX, TXT, ANY)", DefaultValueFactory = _ => "A" };

        var cmd = new BaseCommand("dns", "Perform host/DNS record lookup queries");
        cmd.Arguments.Add(hostArg);
        cmd.Options.Add(typeOption);

        cmd.SetAction(async parseResult =>
        {
            var host = parseResult.GetValue(hostArg) ?? string.Empty;
            var type = parseResult.GetValue(typeOption) ?? "A";
            var res = await networkService.ResolveDnsAsync(host, type);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            if (res.IsSingleTypeQuery)
            {
                if (res.SingleTypeValues.Any())
                {
                    outputProvider.AppendLine(string.Join(Environment.NewLine, res.SingleTypeValues));
                }
                else
                {
                    outputProvider.AppendLine($"No {res.RecordType} records found for {host}.");
                }
            }
            else
            {
                outputProvider.AppendLine($"Host Name: {res.HostName}");
                if (res.Aliases.Any())
                {
                    outputProvider.AppendLine($"Aliases: {string.Join(", ", res.Aliases)}");
                }
                outputProvider.AppendLine("IP Addresses:");
                foreach (var ip in res.Records)
                {
                    outputProvider.AppendLine($"- {ip.Value} ({ip.Family})");
                }
            }
        });

        return cmd;
    }

    private BaseCommand GetPortCommand()
    {
        var hostArg = new Argument<string>("host") { Description = "Host address or IP to scan" };
        var portOption = new Option<int>("--port", "-p") { Description = "Target port to test", Required = true };

        var cmd = new BaseCommand("port", "Scan or test TCP port connectivity of a host");
        cmd.Arguments.Add(hostArg);
        cmd.Options.Add(portOption);

        cmd.SetAction(async parseResult =>
        {
            var host = parseResult.GetValue(hostArg) ?? "localhost";
            var port = parseResult.GetValue(portOption);
            var res = await networkService.ScanPortAsync(host, port);
            outputProvider.AppendLine($"Port {res.Port} on {res.Host} is {res.StatusDetails}.");
        });

        return cmd;
    }

    private BaseCommand GetWhoisCommand()
    {
        var domainArg = new Argument<string>("domain") { Description = "Domain name to query WHOIS registry info" };

        var cmd = new BaseCommand("whois", "Query domain registration metadata (WHOIS info)");
        cmd.Arguments.Add(domainArg);

        cmd.SetAction(async parseResult =>
        {
            var domain = parseResult.GetValue(domainArg) ?? string.Empty;
            var res = await networkService.QueryWhoisAsync(domain);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine(res.RawWhoisText);
        });

        return cmd;
    }

    private BaseCommand GetPingCommand()
    {
        var urlArg = new Argument<string>("url") { Description = "URL or host to ping" };
        var timeoutOption = new Option<int>("--timeout", "-t") { Description = "Timeout in seconds", DefaultValueFactory = _ => 10 };

        var cmd = new BaseCommand("ping", "Perform HTTP ping to measure latency and return response headers");
        cmd.Arguments.Add(urlArg);
        cmd.Options.Add(timeoutOption);

        cmd.SetAction(async parseResult =>
        {
            var url = parseResult.GetValue(urlArg) ?? string.Empty;
            var timeout = parseResult.GetValue(timeoutOption);
            var res = await networkService.HttpPingAsync(url, timeout);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"HTTP Ping to: {res.Url}");
            outputProvider.AppendLine($"Status: {res.StatusCode} ({res.StatusCodeNumber})");
            outputProvider.AppendLine($"Latency: {res.ElapsedMs} ms");
            outputProvider.AppendLine($"Content Length: {(res.ContentLengthBytes.HasValue ? res.ContentLengthBytes.Value.ToString() : "Unknown")} bytes");
            if (!string.IsNullOrEmpty(res.Server))
            {
                outputProvider.AppendLine($"Server: {res.Server}");
            }
            if (!string.IsNullOrEmpty(res.ContentType))
            {
                outputProvider.AppendLine($"Content Type: {res.ContentType}");
            }
        });

        return cmd;
    }

    private BaseCommand GetSslCommand()
    {
        var hostArg = new Argument<string>("hostname") { Description = "Hostname/Domain to inspect SSL cert" };

        var cmd = new BaseCommand("ssl", "Retrieve SSL/TLS certificate details for a domain");
        cmd.Arguments.Add(hostArg);

        cmd.SetAction(async parseResult =>
        {
            var host = parseResult.GetValue(hostArg) ?? string.Empty;
            var res = await networkService.GetSslInfoAsync(host);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"SSL/TLS Certificate Info for: {res.Hostname}");
            outputProvider.AppendLine($"Subject: {res.Subject}");
            outputProvider.AppendLine($"Issuer: {res.Issuer}");
            outputProvider.AppendLine($"Valid From: {res.ValidFrom}");
            outputProvider.AppendLine($"Valid To: {res.ValidTo}");
            outputProvider.AppendLine($"Thumbprint: {res.Thumbprint}");
            outputProvider.AppendLine($"Serial Number: {res.SerialNumber}");
            outputProvider.AppendLine($"Is Expired: {(res.IsExpired ? "Yes" : "No")}");
            outputProvider.AppendLine($"Days until expiry: {res.DaysUntilExpiry}");
        });

        return cmd;
    }

    private BaseCommand GetCidrCommand()
    {
        var cidrArg = new Argument<string>("cidr") { Description = "CIDR notation (e.g. 192.168.1.0/24)" };

        var cmd = new BaseCommand("cidr", "Calculate IP range, broadcast, subnet mask, and host count from CIDR notation");
        cmd.Arguments.Add(cidrArg);

        cmd.SetAction(parseResult =>
        {
            var cidr = parseResult.GetValue(cidrArg) ?? string.Empty;
            var res = networkService.CalculateCidr(cidr);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"CIDR Input: {res.CidrNotation}");
            outputProvider.AppendLine($"Network Address: {res.NetworkAddress}");
            outputProvider.AppendLine($"Broadcast Address: {res.BroadcastAddress}");
            outputProvider.AppendLine($"Subnet Mask: {res.SubnetMask}");
            outputProvider.AppendLine($"First Usable IP: {res.FirstUsableIp}");
            outputProvider.AppendLine($"Last Usable IP: {res.LastUsableIp}");
            outputProvider.AppendLine($"Total Hosts: {res.TotalHosts:N0}");
            outputProvider.AppendLine($"Usable Hosts: {res.UsableHosts:N0}");
        });

        return cmd;
    }

    private BaseCommand GetMacCommand()
    {
        var addressArg = new Argument<string>("address") { Description = "MAC address to lookup" };

        var cmd = new BaseCommand("mac", "Look up the hardware manufacturer/vendor of a MAC address");
        cmd.Arguments.Add(addressArg);

        cmd.SetAction(async parseResult =>
        {
            var addr = parseResult.GetValue(addressArg) ?? string.Empty;
            var res = await networkService.LookupMacVendorAsync(addr);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"MAC: {res.MacAddress}");
            outputProvider.AppendLine($"Vendor: {res.VendorName}");
        });

        return cmd;
    }
}
