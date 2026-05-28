using nHash.Application.Shared.Conversions;
using nHash.Application.Texts.Yaml;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class YamlCommand(IFileProvider fileProvider, IYamlService yamlService, IOutputProvider outputProvider) : IYamlCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text")
    {
        Description = "YAML text for processing",
        DefaultValueFactory = _ => string.Empty
    };

    private readonly Option<string> _fileName = new("--file", "-f")
    {
        Description = "File name for read YAML from that"
    };

    private readonly Option<ConversionType> _conversion = new("--convert", "-c")
    {
        Description = "Convert YAML to other format (JSON, XML)"
    };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("yaml", "YAML tools", GetExamples());

        command.Options.Add(_fileName);
        command.Options.Add(_conversion);
        command.Arguments.Add(_textArgument);

        command.SetAction(async parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var fileName = parseResult.GetValue(_fileName);
            var conversion = parseResult.GetValue(_conversion);
            await CalculateText(text ?? string.Empty, fileName ?? string.Empty, conversion);
        });

        command.Aliases.Add("y");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("To read YAML text from a file", "nhash text yaml -f input.yaml"),
            new("To convert YAML text to JSON", "nhash t y 'name: John Doe\nage: 30' -c json"),
        ];
    
    private async Task CalculateText(string text, string fileName, ConversionType conversion)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var resultText = yamlService.CalculateText(text, conversion);
            WriteOutput(resultText);
            return;
        }

        var fileContent = await fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }

        var fileResultText = yamlService.CalculateText(fileContent, conversion);
        WriteOutput(fileResultText);
    }
    
    private void WriteOutput(string text)
    {
        outputProvider.AppendLine(text);
    }
}