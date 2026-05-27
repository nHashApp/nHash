using System.CommandLine;
using nHash.Application.Texts;
using nHash.Application.Abstraction;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class EscapeCommand(ITextToolsService textToolsService, IOutputProvider outputProvider) : IEscapeCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text") { Description = "Text to escape or unescape" };
    private readonly Option<string> _langOption = new("--lang", "-l") { Description = "Language format: json, csharp, sql, xml", DefaultValueFactory = _ => "json" };
    private readonly Option<bool> _unescapeOption = new("--unescape", "-u") { Description = "Unescape the text instead of escaping", DefaultValueFactory = _ => false };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("escape", "Escape or unescape characters for JSON, C#, SQL, or XML", GetExamples());
        command.Arguments.Add(_textArgument);
        command.Options.Add(_langOption);
        command.Options.Add(_unescapeOption);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument) ?? string.Empty;
            var lang = parseResult.GetValue(_langOption) ?? "json";
            var unescape = parseResult.GetValue(_unescapeOption);
            var result = textToolsService.EscapeString(text, lang, unescape);
            outputProvider.AppendLine(result);
        });
        command.Aliases.Add("esc");
        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Escape text for JSON", "nhash text escape \"Hello\nWorld\""),
            new("Unescape JSON string", "nhash text escape \"Hello\\nWorld\" -u"),
            new("Escape single quotes for SQL", "nhash text escape \"John's Store\" --lang sql")
        ];
}
