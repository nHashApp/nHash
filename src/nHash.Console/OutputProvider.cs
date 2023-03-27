using System.Text;
using nHash.Domain.Models;

namespace nHash.Console;

public class OutputProvider : IOutputProvider
{
    private readonly OutputParameter _outputParameter;
    private readonly StringBuilder _texts;
    private readonly IFileProvider _fileProvider;

    public OutputProvider(OutputParameter outputParameter, IFileProvider fileProvider)
    {
        _outputParameter = outputParameter;
        _fileProvider = fileProvider;
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
        //AnsiConsole.Write(new Text(_texts.ToString()));
        System.Console.Write(_texts.ToString());
    }

    private Task WriteToFile()
    {
        return _fileProvider.Write(_outputParameter.OutputTypeValue, _texts.ToString());
    }
}