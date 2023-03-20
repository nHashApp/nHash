namespace nHash.Domain.Models;

public class OutputParameter
{
    public OutputType Type { get; set; } = OutputType.Console;
    public string OutputTypeValue { get; set; } = string.Empty;
}