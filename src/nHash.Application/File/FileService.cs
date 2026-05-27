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

    public async Task<string> DetectFileTypeAsync(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
            return $"Error: File '{filePath}' does not exist.";

        try
        {
            var info = new FileInfo(filePath);
            byte[] buffer = new byte[16];
            int bytesRead;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                bytesRead = await stream.ReadAsync(buffer, 0, 16);
            }

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

            var sb = new StringBuilder();
            sb.AppendLine($"File: {filePath}");
            sb.AppendLine($"Detected Type: {signature}");
            sb.AppendLine($"File Size: {info.Length:N0} bytes");
            sb.AppendLine($"Extension: {info.Extension}");
            sb.AppendLine($"Last Modified: {info.LastWriteTime}");
            
            var hexBytes = string.Join(" ", buffer.Take(bytesRead).Select(b => b.ToString("X2")));
            sb.AppendLine($"First {bytesRead} Bytes (Hex): {hexBytes}");

            return sb.ToString().TrimEnd();
        }
        catch (Exception ex)
        {
            return $"Error detecting file type: {ex.Message}";
        }
    }

    public string GetDirectoryTree(string directoryPath, int maxDepth, bool showSizes)
    {
        if (!Directory.Exists(directoryPath))
            return $"Error: Directory '{directoryPath}' does not exist.";

        var sb = new StringBuilder();
        var rootDir = new DirectoryInfo(directoryPath);
        sb.AppendLine($"[DIR] {rootDir.Name} ({rootDir.FullName})");

        RenderTree(rootDir, "", sb, 1, maxDepth, showSizes);
        return sb.ToString().TrimEnd();
    }

    private static void RenderTree(DirectoryInfo dirInfo, string indent, StringBuilder sb, int depth, int maxDepth, bool showSizes)
    {
        if (depth > maxDepth) return;

        FileSystemInfo[] children;
        try
        {
            children = dirInfo.GetFileSystemInfos();
        }
        catch
        {
            return;
        }

        var sortedChildren = children
            .OrderBy(c => c is not DirectoryInfo)
            .ThenBy(c => c.Name)
            .ToList();

        for (int i = 0; i < sortedChildren.Count; i++)
        {
            var item = sortedChildren[i];
            bool isLast = i == sortedChildren.Count - 1;
            var marker = isLast ? "└── " : "├── ";

            if (item is DirectoryInfo subDir)
            {
                sb.AppendLine($"{indent}{marker}[DIR] {subDir.Name}");
                var nextIndent = indent + (isLast ? "    " : "│   ");
                RenderTree(subDir, nextIndent, sb, depth + 1, maxDepth, showSizes);
            }
            else if (item is FileInfo file)
            {
                var sizeStr = showSizes ? $" ({file.Length:N0} bytes)" : "";
                sb.AppendLine($"{indent}{marker}{file.Name}{sizeStr}");
            }
        }
    }

    public async Task<string> RenameBatchAsync(string directoryPath, string pattern, string replacement, bool preview, string fileExtensions)
    {
        if (!Directory.Exists(directoryPath))
            return $"Error: Directory '{directoryPath}' does not exist.";

        if (string.IsNullOrEmpty(pattern))
            return "Error: Rename pattern cannot be empty.";

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
            var sb = new StringBuilder();
            sb.AppendLine(preview ? "=== BATCH RENAME PREVIEW ===" : "=== BATCH RENAME EXECUTION ===");

            int renameCount = 0;
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

                    sb.AppendLine($"  {oldName} -> {newName}");
                    renameCount++;

                    if (!preview)
                    {
                        System.IO.File.Move(file.FullName, newPath);
                    }
                }
            }

            sb.AppendLine();
            sb.AppendLine($"Total files affected: {renameCount}");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Batch Rename Error: {ex.Message}";
        }
    }

    public async Task<string> CheckIntegrityAsync(string filePath, string? expectedHash)
    {
        if (!System.IO.File.Exists(filePath))
            return $"Error: File '{filePath}' does not exist.";

        try
        {
            var computedHash = await ComputeFileHashAsync(filePath);
            var sb = new StringBuilder();
            sb.AppendLine($"File: {filePath}");
            sb.AppendLine($"Computed SHA-256: {computedHash}");

            if (!string.IsNullOrWhiteSpace(expectedHash))
            {
                var cleanExpected = expectedHash.Trim().ToLowerInvariant();
                var pass = computedHash == cleanExpected;
                sb.AppendLine($"Expected SHA-256: {cleanExpected}");
                sb.AppendLine($"Verification Result: {(pass ? "PASS" : "FAIL")}");
            }
            else
            {
                var sidecarPath = filePath + ".sha256";
                await System.IO.File.WriteAllTextAsync(sidecarPath, computedHash);
                sb.AppendLine($"Written sidecar file: {sidecarPath}");
            }

            return sb.ToString().TrimEnd();
        }
        catch (Exception ex)
        {
            return $"Integrity Check Error: {ex.Message}";
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

