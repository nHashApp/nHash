using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using nHash.Application.File.Models;

namespace nHash.Application.File;

public class FileService : IFileService
{
    public async Task<FindDuplicatesResult> FindDuplicatesAsync(string directoryPath)
    {
        var result = new FindDuplicatesResult();
        if (!Directory.Exists(directoryPath))
        {
            result.Success = false;
            result.ErrorMessage = $"Error: Directory '{directoryPath}' does not exist.";
            return result;
        }

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
            {
                result.Success = true;
                return result;
            }

            var hashGroups = new Dictionary<string, List<DuplicateFileDetail>>();

            foreach (var fileInfo in potentialDuplicates)
            {
                try
                {
                    var hash = await ComputeFileHashAsync(fileInfo.FullName);
                    if (!hashGroups.ContainsKey(hash))
                    {
                        hashGroups[hash] = new List<DuplicateFileDetail>();
                    }
                    hashGroups[hash].Add(new DuplicateFileDetail
                    {
                        FilePath = fileInfo.FullName,
                        SizeBytes = fileInfo.Length
                    });
                }
                catch
                {
                    // Skip inaccessible files
                }
            }

            result.Groups = hashGroups
                .Where(g => g.Value.Count > 1)
                .Select(g => new DuplicateGroup
                {
                    Hash = g.Key,
                    Files = g.Value
                })
                .ToList();

            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error scanning directory: {ex.Message}";
            return result;
        }
    }

    public async Task<RegexSearchResult> SearchRegexAsync(string directoryPath, string regexPattern, string fileExtensions)
    {
        var result = new RegexSearchResult();
        if (!Directory.Exists(directoryPath))
        {
            result.Success = false;
            result.ErrorMessage = $"Error: Directory '{directoryPath}' does not exist.";
            return result;
        }

        if (string.IsNullOrEmpty(regexPattern))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Regex pattern cannot be empty.";
            return result;
        }

        try
        {
            var regex = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var allowedExtensions = string.IsNullOrWhiteSpace(fileExtensions)
                ? null
                : fileExtensions.Split(',')
                    .Select(ext => ext.Trim().ToLowerInvariant().EnsureStartsWith('.'))
                    .ToHashSet();

            var files = Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories);
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
                    var fileMatches = new List<RegexSearchResultLine>();

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        lineNumber++;
                        if (regex.IsMatch(line))
                        {
                            fileMatches.Add(new RegexSearchResultLine
                            {
                                LineNumber = lineNumber,
                                LineContent = line.Trim()
                            });
                            matchCount++;
                        }
                    }

                    if (fileMatches.Any())
                    {
                        result.Files.Add(new RegexSearchResultFile
                        {
                            FilePath = file,
                            Matches = fileMatches
                        });
                    }
                }
                catch
                {
                    // Ignore inaccessible files
                }
            }

            result.ScannedFilesCount = fileCount;
            result.MatchesCount = matchCount;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Regex Search Error: {ex.Message}";
            return result;
        }
    }

    private static async Task<string> ComputeFileHashAsync(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var hashBytes = await sha256.ComputeHashAsync(stream);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    public async Task<FileTypeResult> DetectFileTypeAsync(string filePath)
    {
        var result = new FileTypeResult { FilePath = filePath };
        if (!System.IO.File.Exists(filePath))
        {
            result.Success = false;
            result.ErrorMessage = $"Error: File '{filePath}' does not exist.";
            return result;
        }

        try
        {
            var info = new FileInfo(filePath);
            result.FileSizeBytes = info.Length;
            result.Extension = info.Extension;
            result.LastModified = info.LastWriteTime;

            byte[] buffer = new byte[16];
            int bytesRead;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                bytesRead = await stream.ReadAsync(buffer, 0, 16);
            }

            result.BytesRead = bytesRead;
            result.HexBytes = string.Join(" ", buffer.Take(bytesRead).Select(b => b.ToString("X2")));

            string signature = "";
            if (bytesRead >= 3 && buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF)
                signature = "JPEG Image";
            else if (bytesRead >= 4 && buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47)
                signature = "PNG Image";
            else if (bytesRead >= 3 && buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46)
                signature = "GIF Image";
            else if (bytesRead >= 4 && buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46)
                signature = "PDF Document";
            else if (bytesRead >= 4 && buffer[0] == 0x50 && buffer[1] == 0x4B && buffer[2] == 0x03 && buffer[3] == 0x04)
                signature = "ZIP Archive (or Office Open XML Document like DOCX/XLSX/PPTX)";
            else if (bytesRead >= 4 && buffer[0] == 0x52 && buffer[1] == 0x61 && buffer[2] == 0x72 && buffer[3] == 0x21)
                signature = "RAR Archive";
            else if (bytesRead >= 2 && buffer[0] == 0x1F && buffer[1] == 0x8B)
                signature = "GZIP Compressed File";
            else if (bytesRead >= 3 && buffer[0] == 0x42 && buffer[1] == 0x5A && buffer[2] == 0x68)
                signature = "BZIP2 Compressed File";
            else if (bytesRead >= 4 && buffer[0] == 0x7F && buffer[1] == 0x45 && buffer[2] == 0x4C && buffer[3] == 0x46)
                signature = "ELF Executable (Linux/Unix)";
            else if (bytesRead >= 2 && buffer[0] == 0x4D && buffer[1] == 0x5A)
                signature = "EXE/DLL Executable (Windows MZ)";
            else if (bytesRead >= 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                signature = "UTF-8 BOM Text File";
            else if (bytesRead >= 4 && buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0xFE && buffer[3] == 0xFF)
                signature = "UTF-32 BE Text File";
            else
            {
                try
                {
                    using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var reader = new StreamReader(stream, Encoding.UTF8);
                    var firstLine = await reader.ReadLineAsync();
                    if (firstLine != null)
                    {
                        signature = $"Text File (First line: \"{(firstLine.Length > 60 ? firstLine[..60] + "..." : firstLine)}\")";
                    }
                    else
                    {
                        signature = "Empty File or Text File";
                    }
                }
                catch
                {
                    signature = "Binary/Unknown Format";
                }
            }

            result.DetectedType = signature;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error detecting file type: {ex.Message}";
            return result;
        }
    }

    public DirectoryTreeResult GetDirectoryTree(string directoryPath, int maxDepth, bool showSizes)
    {
        var result = new DirectoryTreeResult();
        if (!Directory.Exists(directoryPath))
        {
            result.Success = false;
            result.ErrorMessage = $"Error: Directory '{directoryPath}' does not exist.";
            return result;
        }

        try
        {
            var rootDir = new DirectoryInfo(directoryPath);
            result.DirectoryName = rootDir.Name;
            result.DirectoryPath = rootDir.FullName;
            result.Root = BuildTree(rootDir, 1, maxDepth);
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error reading directory tree: {ex.Message}";
            return result;
        }
    }

    private static FileTreeNode BuildTree(DirectoryInfo dirInfo, int depth, int maxDepth)
    {
        var node = new FileTreeNode
        {
            Name = dirInfo.Name,
            IsDirectory = true
        };

        if (depth > maxDepth) return node;

        FileSystemInfo[] children;
        try
        {
            children = dirInfo.GetFileSystemInfos();
        }
        catch
        {
            return node;
        }

        var sortedChildren = children
            .OrderBy(c => c is not DirectoryInfo)
            .ThenBy(c => c.Name)
            .ToList();

        foreach (var item in sortedChildren)
        {
            if (item is DirectoryInfo subDir)
            {
                node.Children.Add(BuildTree(subDir, depth + 1, maxDepth));
            }
            else if (item is FileInfo file)
            {
                node.Children.Add(new FileTreeNode
                {
                    Name = file.Name,
                    IsDirectory = false,
                    SizeBytes = file.Length
                });
            }
        }

        return node;
    }

    public async Task<RenameBatchResult> RenameBatchAsync(string directoryPath, string pattern, string replacement, bool preview, string fileExtensions)
    {
        var result = new RenameBatchResult { IsPreview = preview };
        if (!Directory.Exists(directoryPath))
        {
            result.Success = false;
            result.ErrorMessage = $"Error: Directory '{directoryPath}' does not exist.";
            return result;
        }

        if (string.IsNullOrEmpty(pattern))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Rename pattern cannot be empty.";
            return result;
        }

        try
        {
            var allowedExtensions = string.IsNullOrWhiteSpace(fileExtensions)
                ? null
                : fileExtensions.Split(',')
                    .Select(ext => ext.Trim().ToLowerInvariant().EnsureStartsWith('.'))
                    .ToHashSet();

            var allFiles = Directory.EnumerateFiles(directoryPath, "*", SearchOption.TopDirectoryOnly)
                .Select(f => new FileInfo(f))
                .ToList();

            var regex = new Regex(pattern);

            foreach (var file in allFiles)
            {
                if (allowedExtensions != null)
                {
                    var ext = file.Extension.ToLowerInvariant();
                    if (!allowedExtensions.Contains(ext))
                        continue;
                }

                var oldName = file.Name;
                if (regex.IsMatch(oldName))
                {
                    var newName = regex.Replace(oldName, replacement);
                    var newPath = Path.Combine(file.DirectoryName!, newName);

                    result.RenamedFiles.Add(new RenamePreviewDetail
                    {
                        OriginalName = oldName,
                        NewName = newName
                    });

                    if (!preview)
                    {
                        System.IO.File.Move(file.FullName, newPath);
                    }
                }
            }

            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Batch Rename Error: {ex.Message}";
            return result;
        }
    }

    public async Task<IntegrityResult> CheckIntegrityAsync(string filePath, string? expectedHash)
    {
        var result = new IntegrityResult { FilePath = filePath, ExpectedHash = expectedHash };
        if (!System.IO.File.Exists(filePath))
        {
            result.Success = false;
            result.ErrorMessage = $"Error: File '{filePath}' does not exist.";
            return result;
        }

        try
        {
            var computedHash = await ComputeFileHashAsync(filePath);
            result.ComputedHash = computedHash;

            if (!string.IsNullOrWhiteSpace(expectedHash))
            {
                var cleanExpected = expectedHash.Trim().ToLowerInvariant();
                result.ExpectedHash = cleanExpected;
                result.IsMatch = computedHash == cleanExpected;
            }
            else
            {
                var sidecarPath = filePath + ".sha256";
                await System.IO.File.WriteAllTextAsync(sidecarPath, computedHash);
                result.WrittenSidecarPath = sidecarPath;
            }

            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Integrity Check Error: {ex.Message}";
            return result;
        }
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
