namespace nHash.Infrastructure;

public class FileProvider : IFileProvider
{
    public Task<string> ReadAsText(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine($"File {fileName} does not exists!");
            return Task.FromResult(string.Empty);
        }

        try
        {
            return File.ReadAllTextAsync(fileName);
        }
        catch
        {
            Console.WriteLine("Error reading from '{FileName}'", fileName);
        }
        return Task.FromResult(string.Empty);
    }    
    
    public Task<byte[]> ReadAsByte(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine($"File {fileName} does not exists!");
            return Task.FromResult(Array.Empty<byte>());
        }

        try
        {
            return File.ReadAllBytesAsync(fileName);
        }
        catch
        {
            Console.WriteLine("Error reading from '{FileName}'", fileName);
        }
        return Task.FromResult(Array.Empty<byte>());
    }

    public Task Write(string fileName, string text)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine(text);
            return Task.CompletedTask;
        }

        try
        {
            return File.WriteAllTextAsync(fileName, text);
        }
        catch
        {
            Console.WriteLine("Error writing output to '{FileName}'", fileName);
        }
        return Task.CompletedTask;
    }
    
    public Task Write(byte[] content, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine(content);
            return Task.CompletedTask;
        }

        try
        {
            return File.WriteAllBytesAsync(fileName, content);
        }
        catch
        {
            Console.WriteLine("Error writing output to '{FileName}'", fileName);
        }
        return Task.CompletedTask;
    }
}