using nHash.Application.Texts;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class LanguageCommand(ILanguageDetectorService languageDetectorService, IOutputProvider outputProvider, IFileProvider fileProvider) : ILanguageCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text") { Description = "Text to detect language", DefaultValueFactory = _ => string.Empty };
    private readonly Option<string> _fileOption = new("--file", "-f") { Description = "File path to detect language from" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("lang", "Detect language of a text or file (176 languages supported)", GetExamples());
        command.Options.Add(_fileOption);
        command.Arguments.Add(_textArgument);
        command.SetAction(async parseResult =>
        {
            var text = parseResult.GetValue(_textArgument) ?? string.Empty;
            var file = parseResult.GetValue(_fileOption);
            await DetectLanguage(text, file ?? string.Empty);
        });
        command.Aliases.Add("lg");
        return command;
    }

    private async Task DetectLanguage(string text, string file)
    {
        if (!string.IsNullOrWhiteSpace(file))
        {
            text = await fileProvider.ReadAsText(file);
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            outputProvider.AppendLine("Error: Input text is empty. Please provide either a text argument or specify a file using --file option.");
            return;
        }

        var result = languageDetectorService.DetectLanguage(text);
        if (!result.Success)
        {
            outputProvider.AppendLine($"Error: {result.ErrorMessage}");
            return;
        }

        outputProvider.AppendLine($"Language: {result.LanguageCode}");
        outputProvider.AppendLine($"Confidence: {result.Confidence * 100:0.##}%");
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Detect language of a text", "nhash text lang \"Hello, how are you?\""),
            new("Detect language of a file", "nhash text lang --file \"/path/to/document.txt\"")
        ];
}
