namespace nHash.Application.File;

public interface IFileService
{
    Task<string> FindDuplicatesAsync(string directoryPath);
    Task<string> SearchRegexAsync(string directoryPath, string regexPattern, string fileExtensions);
}
