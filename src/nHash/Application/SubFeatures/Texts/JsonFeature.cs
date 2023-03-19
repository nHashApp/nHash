using nHash.Application.Providers;
using nHash.Application.SubFeatures.Texts.Models;

namespace nHash.Application.SubFeatures.Texts;

public class JsonFeature : IFeature
{
    private readonly Argument<string> _textArgument;
    private readonly Option<JsonPrintType> _printType;
    private readonly Option<string> _fileName;
    private readonly Option<string> _outputFileName;

    public JsonFeature()
    {
        _textArgument = new Argument<string>("text", () => string.Empty, "JSON text for processing");
        _printType = new Option<JsonPrintType>("--print", "Print pretty/Compact JSON representation");
        _fileName = new Option<string>(name: "--file", description: "File name for read JSON from that");
        _outputFileName = new Option<string>(name: "--output", description: "File name for writing output");
    }

    public Command Command => GetFeatureCommand();

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

    private static async Task CalculateText(string text, JsonPrintType printType, string fileName,
        string outputFileName)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var jsonText = CalculateJsonText(text, printType);
            await WriteOutput(jsonText, outputFileName);
            return;
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File {fileName} does not exists!");
                return;
            }

            var fileContent = await File.ReadAllTextAsync(fileName);
            var jsonText = CalculateJsonText(fileContent, printType);
            await WriteOutput(jsonText, outputFileName);
        }
    }

    private static async Task WriteOutput(string text, string outputFileName)
    {
        if (string.IsNullOrWhiteSpace(outputFileName))
        {
            Console.WriteLine(text);
            return;
        }

        try
        {
            await File.WriteAllTextAsync(outputFileName, text);
        }
        catch
        {
            Console.WriteLine($"Error writing output to '{outputFileName}'");
        }
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