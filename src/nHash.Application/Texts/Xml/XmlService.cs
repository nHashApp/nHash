using nHash.Application.Abstraction;
using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Xml;

public class XmlService : IXmlService
{
    private readonly IFileProvider _fileProvider;
    private readonly IOutputProvider _outputProvider;

    public XmlService(IFileProvider fileProvider, IOutputProvider outputProvider)
    {
        _fileProvider = fileProvider;
        _outputProvider = outputProvider;
    }

    public async Task CalculateText(string text, string fileName, ConversionType conversion)
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
        if (conversion != ConversionType.XML)
        {
            text = conversion switch
            {
                ConversionType.YAML => Conversion.ToXml(text, ConversionType.XML),
                ConversionType.JSON => Conversion.ToJson(text, ConversionType.XML),
                _ => text
            };
        }

        _outputProvider.Append(text);
    }

}