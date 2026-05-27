using System.CommandLine;
using nHash.Console.CommandLines.Base;
using Spectre.Console;

namespace nHash.Console.CommandLines.Arts;

public class AsciiCommand : IAsciiCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string?> _textArgument = new("text") { Description = "The text to convert to ASCII art", Arity = ArgumentArity.ZeroOrOne };
    private readonly Option<string> _colorOption = new("--color", "-c") { Description = "Color of the ASCII art (red, green, blue, yellow, magenta, cyan, white)", DefaultValueFactory = _ => "white" };
    private readonly Option<string?> _fontOption = new("--font", "-f") { Description = "Optional name of a font in the 'fonts' folder, or path to a custom .flf font file" };
    private readonly Option<bool> _listOption = new("--list", "-l") { Description = "List all available Figlet fonts in the 'fonts' folder" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("ascii", "Render text as ASCII art using Figlet", GetExamples());
        command.Arguments.Add(_textArgument);
        command.Options.Add(_colorOption);
        command.Options.Add(_fontOption);
        command.Options.Add(_listOption);

        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var colorName = parseResult.GetValue(_colorOption) ?? "white";
            var fontName = parseResult.GetValue(_fontOption);
            var listFonts = parseResult.GetValue(_listOption);

            if (listFonts)
            {
                var fontsDir = GetFontsDirectory();
                if (!System.IO.Directory.Exists(fontsDir))
                {
                    AnsiConsole.MarkupLine($"[yellow]Fonts directory not found at: {fontsDir}[/]");
                    AnsiConsole.MarkupLine("[yellow]Please create a 'fonts' folder and place your .flf Figlet fonts there.[/]");
                    return;
                }

                var fontFiles = System.IO.Directory.GetFiles(fontsDir, "*.flf")
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToList();

                if (fontFiles.Count == 0)
                {
                    AnsiConsole.MarkupLine($"[yellow]No Figlet font (.flf) files found in: {fontsDir}[/]");
                    return;
                }

                AnsiConsole.MarkupLine($"[green]Available Figlet Fonts in '{fontsDir}':[/]");
                foreach (var name in fontFiles)
                {
                    AnsiConsole.MarkupLine($"  - {name}");
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                AnsiConsole.MarkupLine("[red]Error: Input text cannot be empty. Please specify a text to render, or use --list to list available fonts.[/]");
                return;
            }

            try
            {
                FigletText figletText;
                if (!string.IsNullOrWhiteSpace(fontName))
                {
                    string finalFontPath = fontName;
                    if (!System.IO.File.Exists(finalFontPath))
                    {
                        var fontsDir = GetFontsDirectory();
                        var checkPath1 = Path.Combine(fontsDir, fontName);
                        var checkPath2 = Path.Combine(fontsDir, fontName + ".flf");

                        if (System.IO.File.Exists(checkPath2))
                        {
                            finalFontPath = checkPath2;
                        }
                        else if (System.IO.File.Exists(checkPath1))
                        {
                            finalFontPath = checkPath1;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine($"[red]Error: Font '{fontName}' could not be found directly or in the 'fonts' folder ({fontsDir}).[/]");
                            return;
                        }
                    }

                    var font = FigletFont.Load(finalFontPath);
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

    private static string GetFontsDirectory()
    {
        var cwdFonts = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "fonts");
        if (System.IO.Directory.Exists(cwdFonts)) return cwdFonts;

        var baseFonts = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fonts");
        return baseFonts;
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
            new("List all available fonts in the fonts folder", "nhash art ascii --list"),
            new("Render text using 'slant' font from fonts folder", "nhash art ascii \"nHash\" -f slant -c green")
        ];
}
