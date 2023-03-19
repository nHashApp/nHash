namespace nHash.Infrastructure;

public interface IFileProvider
{
    Task<string> ReadAsText(string fileName);
    Task<byte[]> ReadAsByte(string fileName);
    Task Write(string text, string fileName);
}