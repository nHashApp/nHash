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
    private readonly IOutputProvider _outputProvider;
    private readonly IJsonService _jsonService;

    public JsonCommand(IFileProvider fileProvider, IOutputProvider outputProvider, IJsonService jsonService)
    {
        _fileProvider = fileProvider;
        _outputProvider = outputProvider;
        _jsonService = jsonService;
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
        await _jsonService.CalculateText(text, printType, fileName, conversion);
    }
}