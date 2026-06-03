using Spectre.Console;
using nHash.Domain.Models;

namespace nHash.Console.Services;

public class AnsiConsoleProvider : IAnsiConsoleProvider
{
    private readonly OutputParameter _outputParameter;
    private IAnsiConsole? _console;
    private StringWriter? _stringWriter;

    public AnsiConsoleProvider(OutputParameter outputParameter)
    {
        _outputParameter = outputParameter;
    }

    public IAnsiConsole Console
    {
        get
        {
            if (_console == null)
            {
                if (_outputParameter.Type == OutputType.File)
                {
                    _stringWriter = new StringWriter();
                    _console = AnsiConsole.Create(new AnsiConsoleSettings
                    {
                        Ansi = AnsiSupport.No,
                        ColorSystem = ColorSystemSupport.NoColors,
                        Interactive = InteractionSupport.No,
                        Out = new AnsiConsoleOutput(_stringWriter)
                    });
                }
                else
                {
                    _console = AnsiConsole.Console;
                }
            }
            return _console;
        }
    }

    public StringWriter? StringWriter => _stringWriter;
}
