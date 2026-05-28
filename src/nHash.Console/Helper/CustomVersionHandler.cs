using Spectre.Console;

namespace nHash.Console.Helper;

public static class CustomVersionHandler
{
    private const string Version = "version: ";

    public static RootCommand UseCustomVersionHandler(this RootCommand rootCommand)
    {
        var versionOption = rootCommand.Options
            .Where(x => x.Name == "version")
            .Select(x => (Option<bool>)x)
            .FirstOrDefault();
        
        if (versionOption is not null)
        {
            versionOption.Validators.Add(_ =>
            {
                AnsiConsole.Write(new FigletText("nHash").Centered());
                System.Console.SetCursorPosition((System.Console.WindowWidth - Version.Length - 5) / 2,
                    System.Console.CursorTop);
                System.Console.Write(Version);
            });
        }

        return rootCommand;
    }
}