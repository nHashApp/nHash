using nHash.Application.Shared.Conversions;
using nHash.Application.Shared.Json;
using nHash.Application.Texts.Json.Models;

namespace nHash.Application.Texts.Json;

public class JsonFeature : IJsonFeature
{
    public Command Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument;
    private readonly Option<JsonPrintType> _printType;
    private readonly Option<string> _fileName;
    private readonly Option<ConversionType> _conversion;

    private readonly IFileProvider _fileProvider;
    private readonly IOutputProvider _outputProvider;

    public JsonFeature(IFileProvider fileProvider, IOutputProvider outputProvider)
    {
        _fileProvider = fileProvider;
        _outputProvider = outputProvider;
        _textArgument = new Argument<string>("text", () => string.Empty, "JSON text for processing");
        _printType = new Option<JsonPrintType>("--print", "Print pretty/Compact JSON representation");
        _fileName = new Option<string>(name: "--file", description: "File name for read JSON from that");
        _conversion =
            new Option<ConversionType>(name: "--convert", description: "Convert JSON to other format (YAML, XML)");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("json", "JSON tools")
        {
            _printType,
            _fileName,
            _conversion,
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _printType, _fileName, _conversion);

        return command;
    }

    private async Task CalculateText(string text, JsonPrintType printType, string fileName,
        ConversionType conversion)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var jsonText = CalculateJsonText(text, printType);
            WriteOutput(jsonText, conversion);
            return;
        }

        var fileContent = await _fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }

        var jsonFileText = CalculateJsonText(fileContent, printType);
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