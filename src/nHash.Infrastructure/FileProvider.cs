using System.Text;
using nHash.Application.Abstraction;

namespace nHash.Infrastructure;

public class FileProvider : IFileProvider
{
    private readonly IOutputProvider _outputProvider;

    public FileProvider(IOutputProvider outputProvider)
    {
        _outputProvider = outputProvider;
    }

    public Task<string> ReadAsText(string fileName)
    {
        if (!File.Exists(fileName))
        {
            _outputProvider.AppendLine($"File {fileName} does not exist!");
            return Task.FromResult(string.Empty);
        }

        try
        {
            return File.ReadAllTextAsync(fileName);
        }
        catch
        {
            _outputProvider.AppendLine($"Error reading from '{fileName}'");
        }
        return Task.FromResult(string.Empty);
    }    
    
    public Task<byte[]> ReadAsByte(string fileName)
    {
        if (!File.Exists(fileName))
        {
            _outputProvider.AppendLine($"File {fileName} does not exist!");
            return Task.FromResult(Array.Empty<byte>());
        }

        try
        {
            return File.ReadAllBytesAsync(fileName);
        }
        catch
        {
            _outputProvider.AppendLine($"Error reading from '{fileName}'");
        }
        return Task.FromResult(Array.Empty<byte>());
    }

    public Task Write(string fileName, string text)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            _outputProvider.AppendLine(text);
            return Task.CompletedTask;
        }

        try
        {
            return File.WriteAllTextAsync(fileName, text);
        }
        catch
        {
            _outputProvider.AppendLine($"Error writing output to '{fileName}'");
        }
        return Task.CompletedTask;
    }
    
    public Task Write(byte[] content, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            _outputProvider.AppendLine(Encoding.UTF8.GetString(content));
            return Task.CompletedTask;
        }

        try
        {
            return File.WriteAllBytesAsync(fileName, content);
        }
        catch
        {
            _outputProvider.AppendLine($"Error writing output to '{fileName}'");
        }
        return Task.CompletedTask;
    }
}