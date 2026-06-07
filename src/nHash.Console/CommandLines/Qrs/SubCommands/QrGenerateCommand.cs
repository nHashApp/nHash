using nHash.Application.Qrs;
using nHash.Console.CommandLines.Base;
using nHash.Domain.Models;

namespace nHash.Console.CommandLines.Qrs.SubCommands;

public class QrGenerateCommand(
    IQrService qrService,
    IOutputProvider outputProvider,
    IFileProvider fileProvider,
    OutputParameter outputParameter)
    : IQrGenerateCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text")
    {
        Description = "Text or content to encode in the QR code",
        DefaultValueFactory = _ => string.Empty
    };

    private readonly Option<string> _fileOption = new("--file", "-f")
    {
        Description = "File path to read content from"
    };

    private readonly Option<string> _formatOption = new("--format", "-fmt")
    {
        Description = "Output format (console, png, svg)",
        DefaultValueFactory = _ => "console"
    };

    private readonly Option<string> _eccOption = new("--ecc", "-e")
    {
        Description = "Error correction level (low, medium, quartile, high)",
        DefaultValueFactory = _ => "medium"
    };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("generate", "Generate a QR code from text or file input", GetExamples());
        command.Arguments.Add(_textArgument);
        command.Options.Add(_fileOption);
        command.Options.Add(_formatOption);
        command.Options.Add(_eccOption);

        command.SetAction(async parseResult =>
        {
            var text = parseResult.GetValue(_textArgument) ?? string.Empty;
            var file = parseResult.GetValue(_fileOption);
            var format = parseResult.GetValue(_formatOption) ?? "console";
            var ecc = parseResult.GetValue(_eccOption) ?? "medium";

            await GenerateQrCode(text, file ?? string.Empty, format, ecc);
        });

        command.Aliases.Add("gen");
        command.Aliases.Add("create");
        return command;
    }

    private async Task GenerateQrCode(string text, string file, string format, string ecc)
    {
        // 1. Resolve input text
        if (!string.IsNullOrWhiteSpace(file))
        {
            text = await fileProvider.ReadAsText(file);
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            outputProvider.AppendLine("Error: Input text is empty. Please provide either a text argument or specify a file using --file option.");
            return;
        }

        // 2. Validate ECC Level
        QrErrorCorrectionLevel eccLevel;
        switch (ecc.ToLower())
        {
            case "low":
            case "l":
                eccLevel = QrErrorCorrectionLevel.Low;
                break;
            case "medium":
            case "m":
                eccLevel = QrErrorCorrectionLevel.Medium;
                break;
            case "quartile":
            case "q":
                eccLevel = QrErrorCorrectionLevel.Quartile;
                break;
            case "high":
            case "h":
                eccLevel = QrErrorCorrectionLevel.High;
                break;
            default:
                outputProvider.AppendLine($"Error: Invalid error correction level '{ecc}'. Supported values: low (l), medium (m), quartile (q), high (h).");
                return;
        }

        // 3. Generate QR code based on format
        switch (format.ToLower())
        {
            case "console":
                var asciiQr = qrService.GenerateAscii(text, eccLevel);
                outputProvider.Append(asciiQr);
                break;

            case "svg":
                var svgQr = qrService.GenerateSvg(text, eccLevel);
                outputProvider.Append(svgQr);
                break;

            case "png":
                if (string.IsNullOrWhiteSpace(outputParameter.OutputTypeValue))
                {
                    outputProvider.AppendLine("Error: PNG format requires an output file path. Please specify the output file name using the global '--output' (or '-o') option.");
                    return;
                }

                var pngBytes = qrService.GeneratePng(text, eccLevel);
                await fileProvider.Write(pngBytes, outputParameter.OutputTypeValue);
                outputProvider.AppendLine($"QR code successfully saved to PNG file: {outputParameter.OutputTypeValue}");
                
                // Prevent OutputProvider from overwriting the binary file at program completion
                outputParameter.Type = OutputType.Console;
                break;

            default:
                outputProvider.AppendLine($"Error: Invalid output format '{format}'. Supported formats: console, png, svg.");
                break;
        }
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Generate and preview QR code in terminal", "nhash qr generate \"Hello World\""),
            new("Generate QR code from file content", "nhash qr generate -f input.txt"),
            new("Save QR code as SVG to a file", "nhash qr generate \"https://google.com\" --format svg -o qr.svg"),
            new("Save QR code as PNG to a file", "nhash qr generate \"https://google.com\" --format png -o qr.png --ecc high")
        ];
}
