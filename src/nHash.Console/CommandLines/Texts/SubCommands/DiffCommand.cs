using nHash.Application;
using nHash.Application.Texts;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class DiffCommand(ITextDiffService diffService, IOutputProvider outputProvider, IFileProvider fileProvider) : IDiffCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Argument<string> _text1Argument = new("text1") { Description = "First text or file path" };
    private readonly Argument<string> _text2Argument = new("text2") { Description = "Second text or file path" };
    private readonly Option<bool> _isFileOption = new("--file", "-f") { Description = "Compare files instead of raw text strings" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("diff", "Compare two texts or files line-by-line", GetExamples());
        command.Options.Add(_isFileOption);
        command.Arguments.Add(_text1Argument);
        command.Arguments.Add(_text2Argument);
        command.SetAction(async parseResult =>
        {
            var text1 = parseResult.GetValue(_text1Argument) ?? string.Empty;
            var text2 = parseResult.GetValue(_text2Argument) ?? string.Empty;
            var isFile = parseResult.GetValue(_isFileOption);
            await CalculateDiff(text1, text2, isFile);
        });
        command.Aliases.Add("df");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Compare two strings", "nhash text diff \"hello world\" \"hello there\""),
            new("Compare two files", "nhash text diff /path/to/file1.txt /path/to/file2.txt --file"),
        ];

    private async Task CalculateDiff(string t1, string t2, bool isFile)
    {
        if (isFile)
        {
            t1 = await fileProvider.ReadAsText(t1);
            t2 = await fileProvider.ReadAsText(t2);
        }
        var resultText = diffService.Compare(t1, t2);
        outputProvider.AppendLine(resultText);
    }
}
