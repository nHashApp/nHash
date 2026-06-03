using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Cryptos.Hashes.SubCommands;

namespace nHash.Console.CommandLines.Cryptos.Hashes;

public class HashCommand(ICalcCommand calcCommand, IChecksumCommand checksumCommand, IPbkdf2Command pbkdf2Command) : IHashCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        List<IFeature> features =
        [
            calcCommand,
            checksumCommand,
            pbkdf2Command
        ];

        var command = new BaseCommand("hash",
            "Calculate hash, checksum fingerprint, or derive key using PBKDF2 (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32, PBKDF2...)");
        command.Aliases.Add("h");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}