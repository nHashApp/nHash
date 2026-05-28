using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace nHash.Application.Sys;

public class SysService : ISysService
{
    public string GetSystemInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"OS Description: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}");
        sb.AppendLine($"Architecture: {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture}");
        sb.AppendLine($"Framework Description: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
        sb.AppendLine($"Machine Name: {Environment.MachineName}");
        sb.AppendLine($"User Name: {Environment.UserName}");
        sb.AppendLine($"Processor Count: {Environment.ProcessorCount}");
        sb.AppendLine($"System Directory: {Environment.SystemDirectory}");
        sb.AppendLine($"Current Directory: {Environment.CurrentDirectory}");
        
        long workingSetMB = Environment.WorkingSet / (1024 * 1024);
        sb.AppendLine($"Process Working Set: {workingSetMB} MB");

        return sb.ToString().TrimEnd();
    }

    public string GetEnvironmentVariables(string? filter)
    {
        var envs = Environment.GetEnvironmentVariables();
        var sortedList = new List<KeyValuePair<string, string>>();
        foreach (System.Collections.DictionaryEntry entry in envs)
        {
            var key = entry.Key?.ToString() ?? string.Empty;
            var val = entry.Value?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(filter) || key.Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                sortedList.Add(new KeyValuePair<string, string>(key, val));
            }
        }

        sortedList.Sort((x, y) => string.Compare(x.Key, y.Key, StringComparison.OrdinalIgnoreCase));

        var sb = new StringBuilder();
        foreach (var pair in sortedList)
        {
            sb.AppendLine($"{pair.Key}={pair.Value}");
        }

        return sb.ToString().TrimEnd();
    }

    public string GetRunningProcesses(string? filter, int topN)
    {
        var processes = System.Diagnostics.Process.GetProcesses();
        var list = new List<(string Name, int Id, long WorkingSetMB)>();

        foreach (var p in processes)
        {
            try
            {
                var name = p.ProcessName;
                if (string.IsNullOrEmpty(filter) || name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add((name, p.Id, p.WorkingSet64 / (1024 * 1024)));
                }
            }
            catch
            {
                // Ignore processes we can't access
            }
        }

        list.Sort((x, y) => y.WorkingSetMB.CompareTo(x.WorkingSetMB));

        var itemsToShow = list.Take(topN).ToList();

        var sb = new StringBuilder();
        sb.AppendLine($"{"Process Name",-30} | {"Process ID",-10} | {"Memory (MB)",-12}");
        sb.AppendLine(new string('-', 58));
        foreach (var item in itemsToShow)
        {
            sb.AppendLine($"{item.Name,-30} | {item.Id,-10} | {item.WorkingSetMB,-12:N0}");
        }

        return sb.ToString().TrimEnd();
    }
}
