using nHash.Console.Base;
using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Encodes;

namespace nHash.Console.CommandLines.Converts;

public class ConvertCommand(
    IEncodeCommand encodeCommand,
    IFormatCommand formatCommand,
    IBaseNCommand baseNCommand)
    : IConvertCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("convert", "Data and base conversion features");
        command.Aliases.Add("conv");
        command.Subcommands.Add(encodeCommand.Command);
        command.Subcommands.Add(formatCommand.Command);
        command.Subcommands.Add(baseNCommand.Command);

        return command;
    }
}
