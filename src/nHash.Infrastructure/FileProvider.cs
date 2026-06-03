namespace nHash.Infrastructure;

public class FileProvider : IFileProvider
{
    private readonly ILogger<FileProvider>? _logger;

    public FileProvider(ILogger<FileProvider>? logger = null)
    {
        _logger = logger;
    }

    public Task<string> ReadAsText(string fileName)
    {
        if (!File.Exists(fileName))
        {
            var message = $"File {fileName} does not exist!";
            _logger?.LogError(message);
            throw new FileNotFoundException(message, fileName);
        }

        try
        {
            return File.ReadAllTextAsync(fileName);
        }
        catch (Exception ex)
        {
            var message = $"Error reading from '{fileName}': {ex.Message}";
            _logger?.LogError(ex, message);
            throw;
        }
    }    
    
    public Task<byte[]> ReadAsByte(string fileName)
    {
        if (!File.Exists(fileName))
        {
            var message = $"File {fileName} does not exist!";
            _logger?.LogError(message);
            throw new FileNotFoundException(message, fileName);
        }

        try
        {
            return File.ReadAllBytesAsync(fileName);
        }
        catch (Exception ex)
        {
            var message = $"Error reading from '{fileName}': {ex.Message}";
            _logger?.LogError(ex, message);
            throw;
        }
    }

    public Task Write(string fileName, string text)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            var message = "File name cannot be empty or whitespace.";
            _logger?.LogError(message);
            throw new ArgumentException(message, nameof(fileName));
        }

        try
        {
            return File.WriteAllTextAsync(fileName, text);
        }
        catch (Exception ex)
        {
            var message = $"Error writing output to '{fileName}': {ex.Message}";
            _logger?.LogError(ex, message);
            throw;
        }
    }
    
    public Task Write(byte[] content, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            var message = "File name cannot be empty or whitespace.";
            _logger?.LogError(message);
            throw new ArgumentException(message, nameof(fileName));
        }

        try
        {
            return File.WriteAllBytesAsync(fileName, content);
        }
        catch (Exception ex)
        {
            var message = $"Error writing output to '{fileName}': {ex.Message}";
            _logger?.LogError(ex, message);
            throw;
        }
    }
}