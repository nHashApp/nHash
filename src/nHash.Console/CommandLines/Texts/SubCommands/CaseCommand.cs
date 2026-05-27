using nHash.Application.Texts;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class CaseCommand(ICaseConverterService caseService, IOutputProvider outputProvider) : ICaseCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument = new("text") { Description = "Text to convert case" };
    private readonly Option<string> _formatOption = new("--format", "-f") { Description = "Target case format (camel, pascal, snake, kebab, upper, lower)", DefaultValueFactory = _ => "camel" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("case", "Convert text case (camelCase, PascalCase, snake_case, kebab-case, etc.)", GetExamples());
        command.Options.Add(_formatOption);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var format = parseResult.GetValue(_formatOption);
            CalculateText(text ?? string.Empty, format ?? "camel");
        });
        command.Aliases.Add("cs");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Convert text to camelCase", "nhash text case \"hello world\" -f camel"),
            new("Convert text to snake_case", "nhash text case \"Hello World\" --format snake"),
        ];

    private void CalculateText(string text, string format)
    {
        var resultText = caseService.Convert(text, format);
        outputProvider.AppendLine(resultText);
    }
}
