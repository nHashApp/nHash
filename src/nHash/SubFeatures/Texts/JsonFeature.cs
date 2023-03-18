using nHash.Providers;
using nHash.SubFeatures.Texts.Models;

namespace nHash.SubFeatures.Texts;

public class JsonFeature: IFeature
{
    private readonly Argument<string> _textArgument;
    private readonly Option<JsonPrintType> _printType;
    private readonly Option<string> _fileName;

    public JsonFeature()
    {
        _textArgument = new Argument<string>("text",() => string.Empty, "JSON text for processing");
        _printType = new Option<JsonPrintType>("--print", "Print pretty/Compact JSON representation");
        _fileName = new Option<string>(name: "--file", description: "File name for read JSON from that");
    }

    public Command Command => GetFeatureCommand();
    
    private Command GetFeatureCommand()
    {
        var command = new Command("json", "JSON tools")
        {
            _printType,
            _fileName
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _printType, _fileName);

        return command;
    }

    private static void CalculateText(string text, JsonPrintType printType, string fileName)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var jsonText = CalculateJsonText(text, printType);
            Console.WriteLine(jsonText);
            return;
        }
        
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File {fileName} does not exists!");
                return;
            }

            var fileContent = File.ReadAllText(fileName);
            var jsonText = CalculateJsonText(fileContent, printType);
            Console.WriteLine(jsonText);
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