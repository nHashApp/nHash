using nHash.Application.Shared.Conversions;
using nHash.Application.Shared.Json;
using nHash.Application.Texts.Json.Models;

namespace nHash.Application.Texts.Json;

public class JsonService : IJsonService
{
    public string CalculateText(string text, JsonPrintType printType, ConversionType conversion)
    {
        var jsonFileText = CalculateJsonText(text, printType);
        return WriteOutput(jsonFileText, conversion);
    }

    private static string WriteOutput(string text, ConversionType conversion)
    {
        if (conversion != ConversionType.Json)
        {
            text = conversion switch
            {
                ConversionType.Xml => Conversion.ToXml(text, ConversionType.Json),
                ConversionType.Yaml => Conversion.ToYaml(text, ConversionType.Json),
                _ => text
            };
        }

        return text;
    }

    private static string CalculateJsonText(string text, JsonPrintType printType)
    {
        var prettyJson = new JsonTools();
        var jsonText = printType == JsonPrintType.Pretty
            ? prettyJson.SetBeautiful(text)
            : prettyJson.SetCompact(text);
        return jsonText;
    }
}