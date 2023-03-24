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
        if (conversion != ConversionType.YAML)
        {
            text = conversion switch
            {
                ConversionType.XML => Conversion.ToXml(text, ConversionType.YAML),
                ConversionType.JSON => Conversion.ToJson(text, ConversionType.YAML),
                _ => text
            };
        }

        return text;
    }
}