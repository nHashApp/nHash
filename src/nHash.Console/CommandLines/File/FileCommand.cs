using System.CommandLine;
using nHash.Application.File;
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
            outputProvider.AppendLine(res);
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
            outputProvider.AppendLine(res);
        });

        return cmd;
    }
}
