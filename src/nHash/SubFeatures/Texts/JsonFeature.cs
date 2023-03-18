using nHash.Providers;
using nHash.SubFeatures.Texts.Models;

namespace nHash.SubFeatures.Texts;

public class JsonFeature: IFeature
{
    private readonly Argument<string> _textArgument;
    private readonly Option<JsonPrintType> _printType;

    public JsonFeature()
    {
        _textArgument = new Argument<string>("text", "JSON text for processing");
        _printType = new Option<JsonPrintType>("--print", "Print pretty/Compact JSON representation");
    }

    public Command Command => GetFeatureCommand();
    
    private Command GetFeatureCommand()
    {
        var command = new Command("json", "JSON tools")
        {
            _printType
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _printType);

        return command;
    }

    private static void CalculateText(string text, JsonPrintType printType)
    {
        var prettyJson = new JsonTools();
        var jsonText = printType == JsonPrintType.Pretty
            ? prettyJson.SetBeautiful(text)
            : prettyJson.SetCompact(text);
        Console.WriteLine(jsonText);
    }
}