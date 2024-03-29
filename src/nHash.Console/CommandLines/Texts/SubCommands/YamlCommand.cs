using nHash.Application.Shared.Conversions;
using nHash.Application.Texts.Yaml;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class YamlCommand : IYamlCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument;
    private readonly Option<string> _fileName;
    private readonly Option<ConversionType> _conversion;

    private readonly IFileProvider _fileProvider;
    private readonly IYamlService _yamlService;
    private readonly IOutputProvider _outputProvider;

    public YamlCommand(IFileProvider fileProvider, IYamlService yamlService, IOutputProvider outputProvider)
    {
        _fileProvider = fileProvider;
        _yamlService = yamlService;
        _outputProvider = outputProvider;
        _textArgument = new Argument<string>("text", () => string.Empty, "YAML text for processing");
        _fileName = new Option<string>(name: "--file", description: "File name for read YAML from that");
        _fileName.AddAlias("-f");
        _conversion =
            new Option<ConversionType>(name: "--convert", description: "Convert YAML to other format (JSON, XML)");
        _conversion.AddAlias("-c");
    }

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("yaml", "YAML tools", GetExamples())
        {
            _fileName,
            _conversion,
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _fileName, _conversion);
        command.AddAlias("y");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples()
    {
        return new List<KeyValuePair<string, string>>()
        {
            new("To read YAML text from a file", "nhash text yaml -f input.yaml"),
            new("To convert YAML text to JSON", "nhash t y 'name: John Doe\nage: 30' -c json"),
        };
    }
    
    private async Task CalculateText(string text, string fileName, ConversionType conversion)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var resultText =_yamlService.CalculateText(text, conversion);
            WriteOutput(resultText);
            return;
        }

        var fileContent = await _fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }

        var fileResultText =_yamlService.CalculateText(fileContent, conversion);
        WriteOutput(fileResultText);
    }
    
    private void WriteOutput(string text)
    {
        _outputProvider.AppendLine(text);
    }
}