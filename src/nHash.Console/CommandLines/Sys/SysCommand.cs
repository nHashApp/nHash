using nHash.Application.Sys;
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
        cmd.Aliases.Add("i");
        cmd.SetAction(parseResult =>
        {
            var res = sysService.GetSystemInfo();
            outputProvider.AppendLine($"OS Description: {res.OsDescription}");
            outputProvider.AppendLine($"Architecture: {res.Architecture}");
            outputProvider.AppendLine($"Framework Description: {res.FrameworkDescription}");
            outputProvider.AppendLine($"Machine Name: {res.MachineName}");
            outputProvider.AppendLine($"User Name: {res.UserName}");
            outputProvider.AppendLine($"Processor Count: {res.ProcessorCount}");
            outputProvider.AppendLine($"System Directory: {res.SystemDirectory}");
            outputProvider.AppendLine($"Current Directory: {res.CurrentDirectory}");
            outputProvider.AppendLine($"Process Working Set: {res.WorkingSetMB} MB");
        });
        return cmd;
    }

    private BaseCommand GetEnvCommand()
    {
        var filterOption = new Option<string?>("--filter", "-f") { Description = "Filter environment variables by name (case-insensitive contains)" };
        var cmd = new BaseCommand("env", "List environment variables, optionally filtered by name");
        cmd.Options.Add(filterOption);
        cmd.Aliases.Add("e");
        cmd.SetAction(parseResult =>
        {
            var filter = parseResult.GetValue(filterOption);
            var res = sysService.GetEnvironmentVariables(filter);
            foreach (var variable in res.Variables)
            {
                outputProvider.AppendLine($"{variable.Key}={variable.Value}");
            }
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
        cmd.Aliases.Add("ps");
        cmd.Aliases.Add("p");
        cmd.SetAction(parseResult =>
        {
            var filter = parseResult.GetValue(filterOption);
            var top = parseResult.GetValue(topOption);
            var res = sysService.GetRunningProcesses(filter, top);
            
            outputProvider.AppendLine($"{"Process Name",-30} | {"Process ID",-10} | {"Memory (MB)",-12}");
            outputProvider.AppendLine(new string('-', 58));
            foreach (var item in res.Processes)
            {
                outputProvider.AppendLine($"{item.Name,-30} | {item.Id,-10} | {item.WorkingSetMB,-12:N0}");
            }
        });
        return cmd;
    }
}
