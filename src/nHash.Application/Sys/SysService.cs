using System;
using System.Collections.Generic;
using System.Linq;
using nHash.Application.Sys.Models;

namespace nHash.Application.Sys;

public class SysService : ISysService
{
    public SystemInfoResult GetSystemInfo()
    {
        return new SystemInfoResult
        {
            OsDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
            Architecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
            FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            MachineName = Environment.MachineName,
            UserName = Environment.UserName,
            ProcessorCount = Environment.ProcessorCount,
            SystemDirectory = Environment.SystemDirectory,
            CurrentDirectory = Environment.CurrentDirectory,
            WorkingSetMB = Environment.WorkingSet / (1024 * 1024)
        };
    }

    public EnvVariablesResult GetEnvironmentVariables(string? filter)
    {
        var result = new EnvVariablesResult();
        var envs = Environment.GetEnvironmentVariables();
        var list = new List<EnvVariableDetail>();

        foreach (System.Collections.DictionaryEntry entry in envs)
        {
            var key = entry.Key?.ToString() ?? string.Empty;
            var val = entry.Value?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(filter) || key.Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                list.Add(new EnvVariableDetail { Key = key, Value = val });
            }
        }

        list.Sort((x, y) => string.Compare(x.Key, y.Key, StringComparison.OrdinalIgnoreCase));
        result.Variables = list;
        return result;
    }

    public RunningProcessesResult GetRunningProcesses(string? filter, int topN)
    {
        var result = new RunningProcessesResult();
        var processes = System.Diagnostics.Process.GetProcesses();
        var list = new List<ProcessDetail>();

        foreach (var p in processes)
        {
            try
            {
                var name = p.ProcessName;
                if (string.IsNullOrEmpty(filter) || name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(new ProcessDetail
                    {
                        Name = name,
                        Id = p.Id,
                        WorkingSetMB = p.WorkingSet64 / (1024 * 1024)
                    });
                }
            }
            catch
            {
                // Ignore processes we can't access
            }
        }

        list.Sort((x, y) => y.WorkingSetMB.CompareTo(x.WorkingSetMB));
        result.Processes = list.Take(topN).ToList();
        return result;
    }
}
