using nHash.Application;
using nHash.Application.Texts;
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
        var resultText = statsService.Calculate(text);
        outputProvider.AppendLine(resultText);
    }
}
