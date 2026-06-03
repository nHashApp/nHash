using nHash.Application.File;
using nHash.Application.File.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.File;

public class FileCommand(IFileService fileService, IOutputProvider outputProvider) : IFileCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("file", "File and directory scanning utility subcommands");

        command.Subcommands.Add(GetDedupCommand());
        command.Subcommands.Add(GetSearchCommand());
        command.Subcommands.Add(GetTypeCommand());
        command.Subcommands.Add(GetTreeCommand());
        command.Subcommands.Add(GetRenameCommand());
        command.Subcommands.Add(GetIntegrityCommand());

        return command;
    }

    private BaseCommand GetDedupCommand()
    {
        var dirArg = new Argument<string>("directory") { Description = "Directory path to scan for duplicates", Arity = ArgumentArity.ZeroOrOne };

        var cmd = new BaseCommand("dedup", "Find duplicate files in a directory using fast SHA-256 size-grouping checks");
        cmd.Arguments.Add(dirArg);

        cmd.SetAction(async parseResult =>
        {
            var dir = parseResult.GetValue(dirArg) ?? ".";
            var res = await fileService.FindDuplicatesAsync(dir);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            if (!res.Groups.Any())
            {
                outputProvider.AppendLine("No duplicate files found.");
                return;
            }

            outputProvider.AppendLine($"Found {res.Groups.Count} groups of duplicate files:");
            outputProvider.AppendLine();

            int groupIndex = 1;
            foreach (var group in res.Groups)
            {
                outputProvider.AppendLine($"Group {groupIndex++} (Hash: {group.Hash}):");
                foreach (var file in group.Files)
                {
                    outputProvider.AppendLine($"  - {file.FilePath} ({file.SizeBytes:N0} bytes)");
                }
                outputProvider.AppendLine();
            }
        });

        return cmd;
    }

    private BaseCommand GetSearchCommand()
    {
        var dirArg = new Argument<string>("directory") { Description = "Directory path to scan", Arity = ArgumentArity.ZeroOrOne };
        var regexOption = new Option<string>("--regex", "-r") { Description = "Regular expression search pattern", Required = true };
        var extOption = new Option<string>("--ext", "-e") { Description = "Optional comma-separated extensions filter (e.g. cs,json,txt)", DefaultValueFactory = _ => string.Empty };

        var cmd = new BaseCommand("search", "Fast regex-based text search inside files in a directory");
        cmd.Arguments.Add(dirArg);
        cmd.Options.Add(regexOption);
        cmd.Options.Add(extOption);

        cmd.SetAction(async parseResult =>
        {
            var dir = parseResult.GetValue(dirArg) ?? ".";
            var regexPattern = parseResult.GetValue(regexOption) ?? string.Empty;
            var ext = parseResult.GetValue(extOption) ?? string.Empty;

            var res = await fileService.SearchRegexAsync(dir, regexPattern, ext);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"Scanned {res.ScannedFilesCount} files. Found {res.MatchesCount} matches.");
            foreach (var fileResult in res.Files)
            {
                outputProvider.AppendLine($"File: {fileResult.FilePath}");
                foreach (var match in fileResult.Matches)
                {
                    outputProvider.AppendLine($"  Line {match.LineNumber}: {match.LineContent}");
                }
            }
        });

        return cmd;
    }

    private BaseCommand GetTypeCommand()
    {
        var fileArg = new Argument<string>("file") { Description = "Path to the file to detect type" };

        var cmd = new BaseCommand("type", "Detect file type and display metadata");
        cmd.Arguments.Add(fileArg);

        cmd.SetAction(async parseResult =>
        {
            var file = parseResult.GetValue(fileArg) ?? string.Empty;
            var res = await fileService.DetectFileTypeAsync(file);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"File: {res.FilePath}");
            outputProvider.AppendLine($"Detected Type: {res.DetectedType}");
            outputProvider.AppendLine($"File Size: {res.FileSizeBytes:N0} bytes");
            outputProvider.AppendLine($"Extension: {res.Extension}");
            outputProvider.AppendLine($"Last Modified: {res.LastModified}");
            outputProvider.AppendLine($"First {res.BytesRead} Bytes (Hex): {res.HexBytes}");
        });

        return cmd;
    }

    private BaseCommand GetTreeCommand()
    {
        var dirArg = new Argument<string>("directory") { Description = "Directory path to render tree", Arity = ArgumentArity.ZeroOrOne };
        var depthOption = new Option<int>("--depth", "-d") { Description = "Maximum recursion depth", DefaultValueFactory = _ => 3 };
        var sizesOption = new Option<bool>("--sizes", "-s") { Description = "Show file sizes in tree" };

        var cmd = new BaseCommand("tree", "Display directory structure as a visual tree");
        cmd.Arguments.Add(dirArg);
        cmd.Options.Add(depthOption);
        cmd.Options.Add(sizesOption);

        cmd.SetAction(parseResult =>
        {
            var dir = parseResult.GetValue(dirArg) ?? ".";
            var depth = parseResult.GetValue(depthOption);
            var sizes = parseResult.GetValue(sizesOption);

            var res = fileService.GetDirectoryTree(dir, depth, sizes);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            if (res.Root != null)
            {
                outputProvider.AppendLine($"[DIR] {res.DirectoryName} ({res.DirectoryPath})");
                RenderTree(res.Root, "", sizes);
            }
        });

        return cmd;
    }

    private void RenderTree(FileTreeNode node, string indent, bool showSizes)
    {
        for (int i = 0; i < node.Children.Count; i++)
        {
            var child = node.Children[i];
            bool isLast = i == node.Children.Count - 1;
            var marker = isLast ? "└── " : "├── ";

            if (child.IsDirectory)
            {
                outputProvider.AppendLine($"{indent}{marker}[DIR] {child.Name}");
                var nextIndent = indent + (isLast ? "    " : "│   ");
                RenderTree(child, nextIndent, showSizes);
            }
            else
            {
                var sizeStr = showSizes ? $" ({child.SizeBytes:N0} bytes)" : "";
                outputProvider.AppendLine($"{indent}{marker}{child.Name}{sizeStr}");
            }
        }
    }

    private BaseCommand GetRenameCommand()
    {
        var dirArg = new Argument<string>("directory") { Description = "Directory path to rename files in", Arity = ArgumentArity.ZeroOrOne };
        var patternOption = new Option<string>("--pattern", "-p") { Description = "Regex pattern to match in filenames", Required = true };
        var replaceOption = new Option<string>("--replace", "-r") { Description = "Replacement text for the pattern", Required = true };
        var previewOption = new Option<bool>("--preview", "-v") { Description = "Preview changes without actually renaming", DefaultValueFactory = _ => true };
        var extOption = new Option<string>("--ext", "-e") { Description = "Optional comma-separated extensions filter", DefaultValueFactory = _ => string.Empty };

        var cmd = new BaseCommand("rename", "Batch rename files in a directory using regular expressions");
        cmd.Arguments.Add(dirArg);
        cmd.Options.Add(patternOption);
        cmd.Options.Add(replaceOption);
        cmd.Options.Add(previewOption);
        cmd.Options.Add(extOption);

        cmd.SetAction(async parseResult =>
        {
            var dir = parseResult.GetValue(dirArg) ?? ".";
            var pattern = parseResult.GetValue(patternOption) ?? string.Empty;
            var replace = parseResult.GetValue(replaceOption) ?? string.Empty;
            var preview = parseResult.GetValue(previewOption);
            var ext = parseResult.GetValue(extOption) ?? string.Empty;

            var res = await fileService.RenameBatchAsync(dir, pattern, replace, preview, ext);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine(res.IsPreview ? "=== BATCH RENAME PREVIEW ===" : "=== BATCH RENAME EXECUTION ===");
            foreach (var detail in res.RenamedFiles)
            {
                outputProvider.AppendLine($"  {detail.OriginalName} -> {detail.NewName}");
            }
            outputProvider.AppendLine();
            outputProvider.AppendLine($"Total files affected: {res.RenamedFiles.Count}");
        });

        return cmd;
    }

    private BaseCommand GetIntegrityCommand()
    {
        var fileArg = new Argument<string>("file") { Description = "Path to the file to verify/hash" };
        var hashOption = new Option<string?>("--hash", "-h") { Description = "Expected SHA-256 hash to verify against" };

        var cmd = new BaseCommand("integrity", "Calculate file SHA-256 integrity and optionally verify or create sidecar hash file");
        cmd.Arguments.Add(fileArg);
        cmd.Options.Add(hashOption);

        cmd.SetAction(async parseResult =>
        {
            var file = parseResult.GetValue(fileArg) ?? string.Empty;
            var expected = parseResult.GetValue(hashOption);

            var res = await fileService.CheckIntegrityAsync(file, expected);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"File: {res.FilePath}");
            outputProvider.AppendLine($"Computed SHA-256: {res.ComputedHash}");
            if (res.ExpectedHash != null)
            {
                outputProvider.AppendLine($"Expected SHA-256: {res.ExpectedHash}");
                outputProvider.AppendLine($"Verification Result: {(res.IsMatch == true ? "PASS" : "FAIL")}");
            }
            else if (res.WrittenSidecarPath != null)
            {
                outputProvider.AppendLine($"Written sidecar file: {res.WrittenSidecarPath}");
            }
        });

        return cmd;
    }
}
