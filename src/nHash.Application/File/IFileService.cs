namespace nHash.Application.File;

public interface IFileService
{
    Task<string> FindDuplicatesAsync(string directoryPath);
    Task<string> SearchRegexAsync(string directoryPath, string regexPattern, string fileExtensions);
    Task<string> DetectFileTypeAsync(string filePath);
    string GetDirectoryTree(string directoryPath, int maxDepth, bool showSizes);
    Task<string> RenameBatchAsync(string directoryPath, string pattern, string replacement, bool preview, string fileExtensions);
    Task<string> CheckIntegrityAsync(string filePath, string? expectedHash);
}

