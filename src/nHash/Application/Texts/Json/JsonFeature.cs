using nHash.Application.Abstraction;
using nHash.Application.Shared.Json;
using nHash.Application.Texts.Json.Models;

namespace nHash.Application.Texts.Json;

public class JsonFeature : IJsonFeature
{
    public Command Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument;
    private readonly Option<JsonPrintType> _printType;
    private readonly Option<string> _fileName;
    private readonly Option<string> _outputFileName;

    private readonly IFileProvider _fileProvider;

    public JsonFeature(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
        _textArgument = new Argument<string>("text", () => string.Empty, "JSON text for processing");
        _printType = new Option<JsonPrintType>("--print", "Print pretty/Compact JSON representation");
        _fileName = new Option<string>(name: "--file", description: "File name for read JSON from that");
        _outputFileName = new Option<string>(name: "--output", description: "File name for writing output");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("json", "JSON tools")
        {
            _printType,
            _fileName,
            _outputFileName
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _printType, _fileName, _outputFileName);

        return command;
    }

    private async Task CalculateText(string text, JsonPrintType printType, string fileName,
        string outputFileName)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var jsonText = CalculateJsonText(text, printType);
            await WriteOutput(jsonText, outputFileName);
            return;
        }

        var fileContent = await _fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }

        var jsonFileText = CalculateJsonText(fileContent, printType);
        await WriteOutput(jsonFileText, outputFileName);
    }

    private async Task WriteOutput(string text, string outputFileName)
    {
        if (string.IsNullOrWhiteSpace(outputFileName))
        {
            Console.WriteLine(text);
            return;
        }

        await _fileProvider.Write(outputFileName, text);
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