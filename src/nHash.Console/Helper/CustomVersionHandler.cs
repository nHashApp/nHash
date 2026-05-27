using System.CommandLine.Builder;
using Spectre.Console;

namespace nHash.Console.Helper;

public static class CustomVersionHandler
{
    private const string Version = "version: ";

    public static CommandLineBuilder UseCustomVersionHandler(this CommandLineBuilder builder)
    {
        var versionOption = builder.Command
            .Where(x => x.Name == "version")
            .Select(x => (Option<bool>)x)
            .First();
        
        versionOption.AddValidator(_ =>
        {
            AnsiConsole.Write(new FigletText("nHash").Centered());
            System.Console.SetCursorPosition((System.Console.WindowWidth - Version.Length - 5) / 2,
                System.Console.CursorTop);
            System.Console.Write(Version);
        });

        return builder;
    }
}