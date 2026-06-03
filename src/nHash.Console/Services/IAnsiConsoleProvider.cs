using Spectre.Console;

namespace nHash.Console.Services;

public interface IAnsiConsoleProvider
{
    IAnsiConsole Console { get; }
    StringWriter? StringWriter { get; }
}
