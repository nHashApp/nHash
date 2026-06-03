using nHash.Application.Sys.Models;

namespace nHash.Application.Sys;

public interface ISysService
{
    SystemInfoResult GetSystemInfo();
    EnvVariablesResult GetEnvironmentVariables(string? filter);
    RunningProcessesResult GetRunningProcesses(string? filter, int topN);
}
