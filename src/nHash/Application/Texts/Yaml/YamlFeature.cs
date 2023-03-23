using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Yaml;

public class YamlFeature : IYamlFeature
{
    public Command Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument;
    private readonly Option<string> _fileName;
    private readonly Option<ConversionType> _conversion;

    private readonly IFileProvider _fileProvider;
    private readonly IOutputProvider _outputProvider;

    public YamlFeature(IFileProvider fileProvider, IOutputProvider outputProvider)
    {
        _fileProvider = fileProvider;
        _outputProvider = outputProvider;
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
            WriteOutput(text, conversion);
            return;
        }

        var fileContent = await _fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }

        WriteOutput(fileContent, conversion);
    }

    private void WriteOutput(string text, ConversionType conversion)
    {
        if (conversion != ConversionType.YAML)
        {
            text = conversion switch
            {
                ConversionType.XML => Conversion.ToXml(text, ConversionType.YAML),
                ConversionType.JSON => Conversion.ToJson(text, ConversionType.YAML),
                _ => text
            };
        }

        _outputProvider.Append(text);
    }

}