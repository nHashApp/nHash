using System.CommandLine;
using System.Threading.Tasks;
using nHash.Application;
using nHash.Application.Texts;
using nHash.Application.Texts.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class StatsCommand(ITextStatisticsService statsService, IOutputProvider outputProvider, IFileProvider fileProvider) : IStatsCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument = new("text") { Description = "Text or file path", DefaultValueFactory = _ => string.Empty };
    private readonly Option<string> _fileName = new("--file", "-f") { Description = "File path to calculate statistics" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("stats", "Calculate detailed statistics for a text or file (words, lines, characters, bytes)", GetExamples());
        command.Options.Add(_fileName);
        command.Arguments.Add(_textArgument);
        command.SetAction(async parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var file = parseResult.GetValue(_fileName);
            await CalculateStats(text ?? string.Empty, file ?? string.Empty);
        });
        command.Aliases.Add("st");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Get statistics for a text string", "nhash text stats \"Hello World! This is a test string.\""),
            new("Get statistics for a file", "nhash text stats --file /path/to/file.txt"),
        ];

    private async Task CalculateStats(string text, string fileName)
    {
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            text = await fileProvider.ReadAsText(fileName);
        }
        var res = statsService.Calculate(text);
        if (!res.Success)
        {
            outputProvider.AppendLine(res.ErrorMessage);
            return;
        }

        outputProvider.AppendLine($"Lines: {res.LinesCount}");
        outputProvider.AppendLine($"Paragraphs: {res.ParagraphsCount}");
        outputProvider.AppendLine($"Words: {res.WordsCount}");
        outputProvider.AppendLine($"Characters (with spaces): {res.CharactersCountWithSpaces}");
        outputProvider.AppendLine($"Characters (no spaces): {res.CharactersCountNoSpaces}");
        outputProvider.AppendLine($"Bytes (UTF-8): {res.BytesCountUtf8}");
        outputProvider.AppendLine($"Bytes (UTF-16/Unicode): {res.BytesCountUtf16}");
        outputProvider.AppendLine($"Bytes (ASCII): {res.BytesCountAscii}");
    }
}
