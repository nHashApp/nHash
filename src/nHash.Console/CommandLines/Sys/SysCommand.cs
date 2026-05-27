using System.CommandLine;
using nHash.Application.Sys;
using nHash.Application.Abstraction;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Sys;

public class SysCommand(ISysService sysService, IOutputProvider outputProvider) : ISysCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("sys", "System information and environment utilities");
        command.Aliases.Add("system");
        command.Subcommands.Add(GetInfoCommand());
        command.Subcommands.Add(GetEnvCommand());
        command.Subcommands.Add(GetProcessCommand());
        return command;
    }

    private BaseCommand GetInfoCommand()
    {
        var cmd = new BaseCommand("info", "Display system information (OS, CPU, memory, runtime)");
        cmd.SetAction(parseResult =>
        {
            var res = sysService.GetSystemInfo();
            outputProvider.AppendLine(res);
        });
        return cmd;
    }

    private BaseCommand GetEnvCommand()
    {
        var filterOption = new Option<string?>("--filter", "-f") { Description = "Filter environment variables by name (case-insensitive contains)" };
        var cmd = new BaseCommand("env", "List environment variables, optionally filtered by name");
        cmd.Options.Add(filterOption);
        cmd.SetAction(parseResult =>
        {
            var filter = parseResult.GetValue(filterOption);
            var res = sysService.GetEnvironmentVariables(filter);
            outputProvider.AppendLine(res);
        });
        return cmd;
    }

    private BaseCommand GetProcessCommand()
    {
        var filterOption = new Option<string?>("--filter", "-f") { Description = "Filter processes by name (case-insensitive contains)" };
        var topOption = new Option<int>("--top", "-n") { Description = "Number of top processes to show", DefaultValueFactory = _ => 20 };
        var cmd = new BaseCommand("process", "List running processes sorted by memory usage");
        cmd.Options.Add(filterOption);
        cmd.Options.Add(topOption);
        cmd.SetAction(parseResult =>
        {
            var filter = parseResult.GetValue(filterOption);
            var top = parseResult.GetValue(topOption);
            var res = sysService.GetRunningProcesses(filter, top);
            outputProvider.AppendLine(res);
        });
        return cmd;
    }
}
