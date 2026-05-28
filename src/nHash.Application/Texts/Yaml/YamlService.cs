using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Yaml;

public class YamlService : IYamlService
{
    public string CalculateText(string text, ConversionType conversion)
    {
        return WriteOutput(text, conversion);
    }

    private static string WriteOutput(string text, ConversionType conversion)
    {
        if (conversion != ConversionType.Yaml)
        {
            text = conversion switch
            {
                ConversionType.Xml => Conversion.ToXml(text, ConversionType.Yaml),
                ConversionType.Json => Conversion.ToJson(text, ConversionType.Yaml),
                _ => text
            };
        }

        return text;
    }
}