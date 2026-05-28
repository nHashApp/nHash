namespace nHash.Application.Sys;

public interface ISysService
{
    string GetSystemInfo();
    string GetEnvironmentVariables(string? filter);
    string GetRunningProcesses(string? filter, int topN);
}
