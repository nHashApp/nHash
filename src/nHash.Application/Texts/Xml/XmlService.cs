using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Xml;

public class XmlService : IXmlService
{
    public string CalculateText(string text, ConversionType conversion)
    {
        return WriteOutput(text, conversion);
    }

    private static string WriteOutput(string text, ConversionType conversion)
    {
        if (conversion != ConversionType.Xml)
        {
            text = conversion switch
            {
                ConversionType.Yaml => Conversion.ToXml(text, ConversionType.Xml),
                ConversionType.Json => Conversion.ToJson(text, ConversionType.Xml),
                _ => text
            };
        }

        return text;
    }

}