using nHash.Application.Shared.Conversions;
using nHash.Application.Texts.Json;
using nHash.Application.Texts.Json.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class JsonCommand(IFileProvider fileProvider, IJsonService jsonService, IOutputProvider outputProvider) : IJsonCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text")
    {
        Description = "JSON text for processing",
        DefaultValueFactory = _ => string.Empty
    };

    private readonly Option<JsonPrintType> _printType = new("--print")
    {
        Description = "Print pretty/Compact JSON representation"
    };

    private readonly Option<string> _fileName = new("--file", "-f")
    {
        Description = "File name for read JSON from that"
    };

    private readonly Option<ConversionType> _conversion = new("--convert", "-c")
    {
        Description = "Convert JSON to other format (YAML, XML)"
    };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("json", "JSON tools", GetExamples());

        command.Options.Add(_printType);
        command.Options.Add(_fileName);
        command.Options.Add(_conversion);
        command.Arguments.Add(_textArgument);

        command.SetAction(async parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var printType = parseResult.GetValue(_printType);
            var fileName = parseResult.GetValue(_fileName);
            var conversion = parseResult.GetValue(_conversion);
            await CalculateText(text ?? string.Empty, printType, fileName ?? string.Empty, conversion);
        });

        command.Aliases.Add("j");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Convert JSON to YAML", "nhash text json '{\"name\": \"John Doe\", \"age\": 30}' -c yaml"),
            new("Print pretty JSON representation", "nhash text json '{\"name\": \"John Doe\", \"age\": 30}' --print pretty"),
            new("Read JSON from a file", "nhash t j -f data.json"),
        ];

    private async Task CalculateText(string text, JsonPrintType printType, string fileName,
        ConversionType conversion)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var resultText = jsonService.CalculateText(text, printType, conversion);
            WriteOutput(resultText);
            return;
        }

        var fileContent = await fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }

        var fileResultText = jsonService.CalculateText(fileContent, printType, conversion);
        WriteOutput(fileResultText);
    }

    private void WriteOutput(string text)
    {
        outputProvider.AppendLine(text);
    }
}