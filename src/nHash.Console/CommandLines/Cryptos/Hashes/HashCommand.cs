using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Hashes.SubCommands;

namespace nHash.Console.CommandLines.Hashes;

public class HashCommand(ICalcCommand calcCommand, IChecksumCommand checksumCommand) : IHashCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        List<IFeature> features =
        [
            calcCommand,
            checksumCommand
        ];

        var command = new BaseCommand("hash",
            "Calculate hash and checksum fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32, CRC8, ...)");
        command.Aliases.Add("h");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}