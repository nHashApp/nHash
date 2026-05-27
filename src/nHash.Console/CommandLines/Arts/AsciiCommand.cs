using System.CommandLine;
using nHash.Console.CommandLines.Base;
using Spectre.Console;

namespace nHash.Console.CommandLines.Arts;

public class AsciiCommand : IAsciiCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text") { Description = "The text to convert to ASCII art" };
    private readonly Option<string> _colorOption = new("--color", "-c") { Description = "Color of the ASCII art (red, green, blue, yellow, magenta, cyan, white)", DefaultValueFactory = _ => "white" };
    private readonly Option<string?> _fontOption = new("--font", "-f") { Description = "Optional path to a custom .flf font file" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("ascii", "Render text as ASCII art using Figlet", GetExamples());
        command.Arguments.Add(_textArgument);
        command.Options.Add(_colorOption);
        command.Options.Add(_fontOption);

        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var colorName = parseResult.GetValue(_colorOption) ?? "white";
            var fontPath = parseResult.GetValue(_fontOption);

            if (string.IsNullOrWhiteSpace(text))
            {
                AnsiConsole.MarkupLine("[red]Error: Input text cannot be empty.[/]");
                return;
            }

            try
            {
                FigletText figletText;
                if (!string.IsNullOrWhiteSpace(fontPath))
                {
                    var font = FigletFont.Load(fontPath);
                    figletText = new FigletText(font, text);
                }
                else
                {
                    figletText = new FigletText(text);
                }

                var color = ParseColor(colorName);
                figletText.Color(color);

                AnsiConsole.Write(figletText);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error generating ASCII art: {ex.Message}[/]");
            }
        });

        return command;
    }

    private static Color ParseColor(string colorName)
    {
        return colorName.ToLowerInvariant().Trim() switch
        {
            "red" => Color.Red,
            "green" => Color.Green,
            "blue" => Color.Blue,
            "yellow" => Color.Yellow,
            "magenta" => Color.DarkMagenta,
            "cyan" => Color.Cyan,
            "white" => Color.White,
            _ => Color.White
        };
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Render text in white ASCII art", "nhash art ascii \"nHash\""),
            new("Render text in green ASCII art", "nhash art ascii \"nHash\" -c green"),
            new("Render text using a custom Figlet font", "nhash art ascii \"nHash\" -f standard.flf")
        ];
}
