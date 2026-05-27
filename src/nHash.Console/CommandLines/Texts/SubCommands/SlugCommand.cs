using System.CommandLine;
using nHash.Application.Texts;
using nHash.Application.Abstraction;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class SlugCommand(ITextToolsService textToolsService, IOutputProvider outputProvider) : ISlugCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text") { Description = "Text to generate slug from" };
    private readonly Option<string> _separatorOption = new("--sep", "-s") { Description = "Separator character", DefaultValueFactory = _ => "-" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("slug", "Generate a URL-friendly slug from text", GetExamples());
        command.Arguments.Add(_textArgument);
        command.Options.Add(_separatorOption);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument) ?? string.Empty;
            var sep = parseResult.GetValue(_separatorOption) ?? "-";
            var result = textToolsService.GenerateSlug(text, sep);
            outputProvider.AppendLine(result);
        });
        command.Aliases.Add("sl");
        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Generate slug", "nhash text slug \"Hello World!\""),
            new("Generate slug with custom separator", "nhash text slug \"Hello World!\" --sep \"_\"")
        ];
}
