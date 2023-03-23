using nHash.Application.Shared.Conversions;
using nHash.Application.Texts.Yaml;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class YamlCommand : IYamlCommand
{
    public Command Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument;
    private readonly Option<string> _fileName;
    private readonly Option<ConversionType> _conversion;

    private readonly IFileProvider _fileProvider;
    private readonly IYamlService _yamlService;

    public YamlCommand(IFileProvider fileProvider, IYamlService yamlService)
    {
        _fileProvider = fileProvider;
        _yamlService = yamlService;
        _textArgument = new Argument<string>("text", () => string.Empty, "YAML text for processing");
        _fileName = new Option<string>(name: "--file", description: "File name for read YAML from that");
        _conversion =
            new Option<ConversionType>(name: "--convert", description: "Convert YAML to other format (JSON, XML)");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("yaml", "YAML tools")
        {
            _fileName,
            _conversion,
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _fileName, _conversion);

        return command;
    }

    private async Task CalculateText(string text, string fileName, ConversionType conversion)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            _yamlService.CalculateText(text, conversion);
            return;
        }

        var fileContent = await _fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }

        _yamlService.CalculateText(fileContent, conversion);
    }
}