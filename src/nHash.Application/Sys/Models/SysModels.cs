namespace nHash.Application.Sys.Models;

public class SystemInfoResult
{
    public string OsDescription { get; set; } = string.Empty;
    public string Architecture { get; set; } = string.Empty;
    public string FrameworkDescription { get; set; } = string.Empty;
    public string MachineName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int ProcessorCount { get; set; }
    public string SystemDirectory { get; set; } = string.Empty;
    public string CurrentDirectory { get; set; } = string.Empty;
    public long WorkingSetMB { get; set; }
}

public class EnvVariableDetail
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class EnvVariablesResult
{
    public List<EnvVariableDetail> Variables { get; set; } = new();
}

public class ProcessDetail
{
    public string Name { get; set; } = string.Empty;
    public int Id { get; set; }
    public long WorkingSetMB { get; set; }
}

public class RunningProcessesResult
{
    public List<ProcessDetail> Processes { get; set; } = new();
}
