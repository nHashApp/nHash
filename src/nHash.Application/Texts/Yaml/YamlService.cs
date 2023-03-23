using nHash.Application.Abstraction;
using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Yaml;

public class YamlService : IYamlService
{
    private readonly IFileProvider _fileProvider;
    private readonly IOutputProvider _outputProvider;

    public YamlService(IFileProvider fileProvider, IOutputProvider outputProvider)
    {
        _fileProvider = fileProvider;
        _outputProvider = outputProvider;
    }

    public void CalculateText(string text, ConversionType conversion)
    {
        WriteOutput(text, conversion);
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