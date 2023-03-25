using nHash.Application.Shared.Conversions;
using nHash.Application.Texts.Json;
using nHash.Application.Texts.Json.Models;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class JsonCommand : IJsonCommand
{
    public Command Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument;
    private readonly Option<JsonPrintType> _printType;
    private readonly Option<string> _fileName;
    private readonly Option<ConversionType> _conversion;

    private readonly IFileProvider _fileProvider;
    private readonly IJsonService _jsonService;
    private readonly IOutputProvider _outputProvider;

    public JsonCommand(IFileProvider fileProvider, IJsonService jsonService, IOutputProvider outputProvider)
    {
        _fileProvider = fileProvider;
        _jsonService = jsonService;
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
            var resultText = _jsonService.CalculateText(text, printType, conversion);
            WriteOutput(resultText);
            return;
        }

        var fileContent = await _fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }

        var fileResultText = _jsonService.CalculateText(fileContent, printType, conversion);
        WriteOutput(fileResultText);
    }

    private void WriteOutput(string text)
    {
        _outputProvider.AppendLine(text);
    }
}