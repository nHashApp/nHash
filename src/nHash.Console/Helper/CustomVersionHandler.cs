using System.CommandLine.Builder;
using Spectre.Console;

namespace nHash.Console.Helper;

public static class CustomVersionHandler
{
    const string version = "version: ";

    public static CommandLineBuilder UseCustomVersionHandler(this CommandLineBuilder builder)
    {
        var versionOption = builder.Command.Where(_ => _.Name == "version").Select(_ => (Option<bool>)_).First();
        versionOption.AddValidator(result =>
        {
            AnsiConsole.Write(new FigletText("nHash").Centered());
            System.Console.SetCursorPosition((System.Console.WindowWidth - version.Length - 5) / 2,
                System.Console.CursorTop);
            System.Console.Write(version);
        });

        return builder;
    }
}