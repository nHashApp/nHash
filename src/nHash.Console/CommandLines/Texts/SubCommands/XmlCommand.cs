using nHash.Application.Shared.Conversions;
using nHash.Application.Texts.Xml;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class XmlCommand : IXmlCommand
{
    public Command Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument;
    private readonly Option<string> _fileName;
    private readonly Option<ConversionType> _conversion;

    private readonly IFileProvider _fileProvider;
    private readonly IXmlService _xmlService;

    public XmlCommand(IFileProvider fileProvider, IXmlService xmlService)
    {
        _fileProvider = fileProvider;
        _xmlService = xmlService;
        _textArgument = new Argument<string>("text", () => string.Empty, "XML text for processing");
        _fileName = new Option<string>(name: "--file", description: "File name for read XML from that");
        _conversion =
            new Option<ConversionType>(name: "--convert", description: "Convert XML to other format (JSON, YAML)");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("xml", "XML tools")
        {
            _fileName,
            _conversion,
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateText, _textArgument, _fileName, _conversion);

        return command;
    }

    private async Task CalculateText(string text, string fileName, ConversionType conversion)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            _xmlService.CalculateText(text, conversion);
            return;
        }

        var fileContent = await _fileProvider.ReadAsText(fileName);
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return;
        }
        _xmlService.CalculateText(fileContent, conversion);
    }
}