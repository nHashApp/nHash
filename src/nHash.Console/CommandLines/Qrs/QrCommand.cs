using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Qrs.SubCommands;

namespace nHash.Console.CommandLines.Qrs;

public class QrCommand(IQrGenerateCommand generateCommand) : IQrCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("qr", "QR Code generation utilities");
        command.Aliases.Add("qrcode");
        command.Subcommands.Add(generateCommand.Command);

        return command;
    }
}
