using nHash.Application.File.Models;

namespace nHash.Application.File;

public interface IFileService
{
    Task<FindDuplicatesResult> FindDuplicatesAsync(string directoryPath);
    Task<RegexSearchResult> SearchRegexAsync(string directoryPath, string regexPattern, string fileExtensions);
    Task<FileTypeResult> DetectFileTypeAsync(string filePath);
    DirectoryTreeResult GetDirectoryTree(string directoryPath, int maxDepth, bool showSizes);
    Task<RenameBatchResult> RenameBatchAsync(string directoryPath, string pattern, string replacement, bool preview, string fileExtensions);
    Task<IntegrityResult> CheckIntegrityAsync(string filePath, string? expectedHash);
}


