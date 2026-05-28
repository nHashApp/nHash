using nHash.Application.Texts;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class LoremCommand(ILoremIpsumService loremService, IOutputProvider outputProvider) : ILoremCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<int> _countOption = new("--count", "-c") { Description = "Number of paragraphs or words to generate", DefaultValueFactory = _ => 1 };
    private readonly Option<string> _typeOption = new("--type", "-t") { Description = "Generation type: 'word' or 'paragraph'", DefaultValueFactory = _ => "paragraph" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("lorem", "Generate standard Lorem Ipsum placeholder text (paragraphs or words)", GetExamples());
        command.Options.Add(_countOption);
        command.Options.Add(_typeOption);
        command.SetAction(parseResult =>
        {
            var count = parseResult.GetValue(_countOption);
            var type = parseResult.GetValue(_typeOption);
            GenerateLorem(count, type ?? "paragraph");
        });
        command.Aliases.Add("lr");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Generate 3 paragraphs of Lorem Ipsum", "nhash text lorem -c 3"),
            new("Generate 50 random words of Lorem Ipsum", "nhash text lorem --count 50 --type word"),
        ];

    private void GenerateLorem(int count, string type)
    {
        var resultText = loremService.Generate(count, type);
        outputProvider.AppendLine(resultText);
    }
}
