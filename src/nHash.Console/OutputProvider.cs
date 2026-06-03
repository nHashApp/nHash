using System.Text;
using nHash.Domain.Models;
using nHash.Console.Services;

namespace nHash.Console;

public class OutputProvider : IOutputProvider
{
    private readonly OutputParameter _outputParameter;
    private readonly StringBuilder _texts;
    private readonly IFileProvider _fileProvider;
    private readonly IAnsiConsoleProvider _ansiConsoleProvider;

    public OutputProvider(OutputParameter outputParameter, IFileProvider fileProvider, IAnsiConsoleProvider ansiConsoleProvider)
    {
        _outputParameter = outputParameter;
        _fileProvider = fileProvider;
        _ansiConsoleProvider = ansiConsoleProvider;
        _texts = new StringBuilder();
    }

    public void Append(string text)
    {
        _texts.Append(text);
    }
    
    public void AppendLine(string text)
    {
        _texts.AppendLine(text);
    }    
    
    public void AppendLine()
    {
        AppendLine(string.Empty);
    }

    public Task WriteOutput()
    {
        if (_outputParameter.Type == OutputType.File)
        {
            return WriteToFile();
        }
        
        WriteToConsole();
        return Task.CompletedTask;
    }

    private void WriteToConsole()
    {
        System.Console.Write(_texts.ToString());
    }

    private Task WriteToFile()
    {
        var consoleText = _ansiConsoleProvider.StringWriter?.ToString() ?? string.Empty;
        var fullText = _texts.ToString() + consoleText;
        return _fileProvider.Write(_outputParameter.OutputTypeValue, fullText);
    }
}