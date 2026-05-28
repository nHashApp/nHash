using Spectre.Console;
using nHash.Console.CommandLines.Base;
using nHash.Application.Abstraction;
using System.CommandLine;

namespace nHash.Console.CommandLines.Arts;

public class ArtCommand(IAsciiCommand asciiCommand) : IArtCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("art", "ASCII art and terminal graphic generators");
        command.Subcommands.Add(asciiCommand.Command);
        command.Subcommands.Add(GetBoxCommand());
        command.Subcommands.Add(GetTableCommand());
        command.Subcommands.Add(GetGradientCommand());
        return command;
    }

    private BaseCommand GetBoxCommand()
    {
        var textArg = new Argument<string>("text") { Description = "Text to display in a box" };
        var titleOption = new Option<string>("--title", "-t") { Description = "Optional box title", DefaultValueFactory = _ => string.Empty };
        var borderOption = new Option<string>("--border", "-b") { Description = "Border style: single, double, rounded, heavy, ascii", DefaultValueFactory = _ => "rounded" };

        var cmd = new BaseCommand("box", "Render text inside a styled terminal box");
        cmd.Arguments.Add(textArg);
        cmd.Options.Add(titleOption);
        cmd.Options.Add(borderOption);
        cmd.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(textArg) ?? string.Empty;
            var title = parseResult.GetValue(titleOption) ?? string.Empty;
            var border = parseResult.GetValue(borderOption) ?? "rounded";
            
            var panelBorder = border.ToLowerInvariant() switch
            {
                "double" => BoxBorder.Double,
                "heavy" => BoxBorder.Heavy,
                "ascii" => BoxBorder.Ascii,
                "single" => BoxBorder.Square,
                _ => BoxBorder.Rounded,
            };
            
            var panel = new Panel(text).Border(panelBorder).Padding(1, 0);
            if (!string.IsNullOrEmpty(title))
                panel = panel.Header(title);
            AnsiConsole.Write(panel);
        });
        return cmd;
    }

    private BaseCommand GetTableCommand()
    {
        var dataArg = new Argument<string>("data") { Description = "CSV data: first row is headers, subsequent rows are data" };
        var borderOption = new Option<string>("--border", "-b") { Description = "Border style: rounded, double, heavy, ascii", DefaultValueFactory = _ => "rounded" };

        var cmd = new BaseCommand("table", "Render CSV data as a beautiful terminal table");
        cmd.Arguments.Add(dataArg);
        cmd.Options.Add(borderOption);
        cmd.SetAction(parseResult =>
        {
            var data = parseResult.GetValue(dataArg) ?? string.Empty;
            var border = parseResult.GetValue(borderOption) ?? "rounded";
            
            var lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) return;
            
            var tableBorder = border.ToLowerInvariant() switch
            {
                "double" => TableBorder.DoubleEdge,
                "heavy" => TableBorder.Heavy,
                "ascii" => TableBorder.Ascii,
                _ => TableBorder.Rounded,
            };
            
            var table = new Table().Border(tableBorder);
            var headers = lines[0].Split(',');
            foreach (var h in headers)
                table.AddColumn(h.Trim());
            
            for (int i = 1; i < lines.Length; i++)
            {
                var cells = lines[i].Split(',');
                table.AddRow(cells.Select(c => c.Trim()).ToArray());
            }
            
            AnsiConsole.Write(table);
        });
        return cmd;
    }

    private BaseCommand GetGradientCommand()
    {
        var textArg = new Argument<string>("text") { Description = "Text to render with color gradient" };
        var fromOption = new Option<string>("--from", "-f") { Description = "Start color (hex like #FF0000 or color name)", DefaultValueFactory = _ => "#FF6B6B" };
        var toOption = new Option<string>("--to", "-t") { Description = "End color (hex like #0000FF or color name)", DefaultValueFactory = _ => "#6BCB77" };

        var cmd = new BaseCommand("gradient", "Render text with a horizontal color gradient in the terminal");
        cmd.Arguments.Add(textArg);
        cmd.Options.Add(fromOption);
        cmd.Options.Add(toOption);
        cmd.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(textArg) ?? string.Empty;
            var from = parseResult.GetValue(fromOption) ?? "#FF6B6B";
            var to = parseResult.GetValue(toOption) ?? "#6BCB77";
            
            static (int r, int g, int b) ParseHex(string hex)
            {
                hex = hex.TrimStart('#');
                if (hex.Length == 6)
                    return (Convert.ToInt32(hex[..2], 16), Convert.ToInt32(hex[2..4], 16), Convert.ToInt32(hex[4..6], 16));
                return (255, 107, 107);
            }
            
            var (r1, g1, b1) = ParseHex(from);
            var (r2, g2, b2) = ParseHex(to);
            
            if (text.Length == 0) return;
            
            for (int i = 0; i < text.Length; i++)
            {
                float t = text.Length > 1 ? (float)i / (text.Length - 1) : 0;
                int r = (int)(r1 + (r2 - r1) * t);
                int g = (int)(g1 + (g2 - g1) * t);
                int b = (int)(b1 + (b2 - b1) * t);
                System.Console.Write($"\x1b[38;2;{r};{g};{b}m{text[i]}");
            }
            System.Console.Write("\x1b[0m");
            System.Console.WriteLine();
        });
        return cmd;
    }
}

