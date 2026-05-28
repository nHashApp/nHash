using nHash.Application.Shared.Conversions;
using nHash.Application.Texts.Xml;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class XmlCommand(IFileProvider fileProvider, IXmlService xmlService, IOutputProvider outputProvider) : IXmlCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text")
    {
        Description = "XML text for processing",
        DefaultValueFactory = _ => string.Empty
    };

    private readonly Option<string> _fileName = new("--file", "-f")
    {
        Description = "File name for read XML from that"
    };

    private readonly Option<ConversionType> _conversion = new("--convert", "-c")
    {
        Description = "Convert XML to other format (JSON, YAML)"
    };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("xml", "XML tools", GetExamples());

        command.Options.Add(_fileName);
        command.Options.Add(_conversion);
        command.Arguments.Add(_textArgument);

        command.SetAction(async parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var fileName = parseResult.GetValue(_fileName);
            var conversion = parseResult.GetValue(_conversion);
            await CalculateText(text ?? string.Empty, fileName ?? string.Empty, conversion);
        });

        command.Aliases.Add("x");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Converting XML to JSON", "nhash text xml \"<person><name>John</name><age>35</age></person>\" --convert json"),
            new("Reading XML from a file", "nhash text xml -f mydata.xml -c yaml --output mydata.yaml"),
        ];

    private async Task CalculateText(string text, string fileName, ConversionType conversion)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var resultText = xmlService.CalculateText(text, conversion);
            WriteOutput(resultText);
            return;
        }

        var fileContent = await fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }

        var fileResultText = xmlService.CalculateText(fileContent, conversion);
        WriteOutput(fileResultText);
    }

    private void WriteOutput(string text)
    {
        outputProvider.AppendLine(text);
    }
}