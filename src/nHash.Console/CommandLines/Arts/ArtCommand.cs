using nHash.Console.Base;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Arts;

public class ArtCommand(IAsciiCommand asciiCommand) : IArtCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("art", "ASCII art and terminal graphic generators");
        command.Subcommands.Add(asciiCommand.Command);
        return command;
    }
}
