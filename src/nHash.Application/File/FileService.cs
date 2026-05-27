using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace nHash.Application.File;

public class FileService : IFileService
{
    public async Task<string> FindDuplicatesAsync(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            return $"Error: Directory '{directoryPath}' does not exist.";

        try
        {
            var allFiles = Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f))
                .ToList();

            var potentialDuplicates = allFiles
                .GroupBy(f => f.Length)
                .Where(g => g.Count() > 1 && g.Key > 0)
                .SelectMany(g => g)
                .ToList();

            if (!potentialDuplicates.Any())
                return "No duplicate files found.";

            var hashGroups = new Dictionary<string, List<string>>();

            foreach (var fileInfo in potentialDuplicates)
            {
                try
                {
                    var hash = await ComputeFileHashAsync(fileInfo.FullName);
                    if (!hashGroups.ContainsKey(hash))
                    {
                        hashGroups[hash] = new List<string>();
                    }
                    hashGroups[hash].Add(fileInfo.FullName);
                }
                catch
                {
                    // Skip inaccessible files
                }
            }

            var duplicateGroups = hashGroups
                .Where(g => g.Value.Count > 1)
                .ToList();

            if (!duplicateGroups.Any())
                return "No duplicate files found.";

            var sb = new StringBuilder();
            sb.AppendLine($"Found {duplicateGroups.Count} groups of duplicate files:");
            sb.AppendLine();

            int groupIndex = 1;
            foreach (var group in duplicateGroups)
            {
                sb.AppendLine($"Group {groupIndex++} (Hash: {group.Key}):");
                foreach (var filePath in group.Value)
                {
                    var info = new FileInfo(filePath);
                    sb.AppendLine($"  - {filePath} ({info.Length:N0} bytes)");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Error scanning directory: {ex.Message}";
        }
    }

    public async Task<string> SearchRegexAsync(string directoryPath, string regexPattern, string fileExtensions)
    {
        if (!Directory.Exists(directoryPath))
            return $"Error: Directory '{directoryPath}' does not exist.";

        if (string.IsNullOrEmpty(regexPattern))
            return "Error: Regex pattern cannot be empty.";

        try
        {
            var regex = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var allowedExtensions = string.IsNullOrWhiteSpace(fileExtensions)
                ? null
                : fileExtensions.Split(',')
                    .Select(ext => ext.Trim().ToLowerInvariant().EnsureStartsWith('.'))
                    .ToHashSet();

            var files = Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories);
            var sb = new StringBuilder();
            int matchCount = 0;
            int fileCount = 0;

            foreach (var file in files)
            {
                if (allowedExtensions != null)
                {
                    var ext = Path.GetExtension(file).ToLowerInvariant();
                    if (!allowedExtensions.Contains(ext))
                        continue;
                }

                fileCount++;
                try
                {
                    using var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var reader = new StreamReader(stream, Encoding.UTF8);

                    int lineNumber = 0;
                    string? line;
                    bool fileHeaderPrinted = false;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        lineNumber++;
                        if (regex.IsMatch(line))
                        {
                            if (!fileHeaderPrinted)
                            {
                                sb.AppendLine($"File: {file}");
                                fileHeaderPrinted = true;
                            }
                            sb.AppendLine($"  Line {lineNumber}: {line.Trim()}");
                            matchCount++;
                        }
                    }
                }
                catch
                {
                    // Ignore inaccessible files
                }
            }

            var header = $"Scanned {fileCount} files. Found {matchCount} matches.\n";
            return header + sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Regex Search Error: {ex.Message}";
        }
    }

    private static async Task<string> ComputeFileHashAsync(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var hashBytes = await sha256.ComputeHashAsync(stream);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}

public static class ExtensionHelper
{
    public static string EnsureStartsWith(this string text, char prefix)
    {
        if (string.IsNullOrEmpty(text)) return text;
        return text[0] == prefix ? text : prefix + text;
    }
}
