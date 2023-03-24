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
        if (conversion != ConversionType.XML)
        {
            text = conversion switch
            {
                ConversionType.YAML => Conversion.ToXml(text, ConversionType.XML),
                ConversionType.JSON => Conversion.ToJson(text, ConversionType.XML),
                _ => text
            };
        }

        return text;
    }

}