using nHash.Application.Abstraction;
using nHash.Application.Shared.Conversions;
using nHash.Application.Shared.Json;
using nHash.Application.Texts.Json.Models;

namespace nHash.Application.Texts.Json;

public class JsonService : IJsonService
{

    private readonly IOutputProvider _outputProvider;

    public JsonService(IOutputProvider outputProvider)
    {
        _outputProvider = outputProvider;
    }


    public void CalculateText(string text, JsonPrintType printType, ConversionType conversion)
    {
        var jsonFileText = CalculateJsonText(text, printType);
        WriteOutput(jsonFileText, conversion);
    }

    private void WriteOutput(string text, ConversionType conversion)
    {
        if (conversion != ConversionType.JSON)
        {
            text = conversion switch
            {
                ConversionType.XML => Conversion.ToXml(text, ConversionType.JSON),
                ConversionType.YAML => Conversion.ToYaml(text, ConversionType.JSON),
                _ => text
            };
        }

        _outputProvider.Append(text);
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