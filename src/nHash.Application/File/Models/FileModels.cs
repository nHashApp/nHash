namespace nHash.Application.File.Models;

public class DuplicateFileDetail
{
    public string FilePath { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
}

public class DuplicateGroup
{
    public string Hash { get; set; } = string.Empty;
    public List<DuplicateFileDetail> Files { get; set; } = new();
}

public class FindDuplicatesResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public List<DuplicateGroup> Groups { get; set; } = new();
}

public class RegexSearchResultLine
{
    public int LineNumber { get; set; }
    public string LineContent { get; set; } = string.Empty;
}

public class RegexSearchResultFile
{
    public string FilePath { get; set; } = string.Empty;
    public List<RegexSearchResultLine> Matches { get; set; } = new();
}

public class RegexSearchResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int ScannedFilesCount { get; set; }
    public int MatchesCount { get; set; }
    public List<RegexSearchResultFile> Files { get; set; } = new();
}

public class FileTypeResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string DetectedType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string Extension { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }
    public string HexBytes { get; set; } = string.Empty;
    public int BytesRead { get; set; }
}

public class FileTreeNode
{
    public string Name { get; set; } = string.Empty;
    public bool IsDirectory { get; set; }
    public long SizeBytes { get; set; }
    public List<FileTreeNode> Children { get; set; } = new();
}

public class DirectoryTreeResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string DirectoryName { get; set; } = string.Empty;
    public string DirectoryPath { get; set; } = string.Empty;
    public FileTreeNode? Root { get; set; }
}

public class RenamePreviewDetail
{
    public string OriginalName { get; set; } = string.Empty;
    public string NewName { get; set; } = string.Empty;
}

public class RenameBatchResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsPreview { get; set; }
    public List<RenamePreviewDetail> RenamedFiles { get; set; } = new();
}

public class IntegrityResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ComputedHash { get; set; } = string.Empty;
    public string? ExpectedHash { get; set; }
    public bool? IsMatch { get; set; }
    public string? WrittenSidecarPath { get; set; }
}
