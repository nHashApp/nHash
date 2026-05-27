using System.CommandLine;
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
            outputProvider.AppendLine(res);
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
            outputProvider.AppendLine(res);
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
            outputProvider.AppendLine(res);
        });

        return cmd;
    }
}
