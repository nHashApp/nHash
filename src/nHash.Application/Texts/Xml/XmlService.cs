using nHash.Application.Abstraction;
using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Xml;

public class XmlService : IXmlService
{
    private readonly IOutputProvider _outputProvider;

    public XmlService(IOutputProvider outputProvider)
    {
        _outputProvider = outputProvider;
    }

    public void CalculateText(string text, ConversionType conversion)
    {
        WriteOutput(text, conversion);
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