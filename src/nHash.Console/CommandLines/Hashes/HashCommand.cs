using nHash.Console.CommandLines.Hashes.SubCommands;

namespace nHash.Console.CommandLines.Hashes;

public class HashCommand : IHashCommand
{
    public Command Command => GetCommand();

    private readonly ICalcCommand _calcCommand;
    private readonly IChecksumCommand _checksumCommand;

    public HashCommand(ICalcCommand calcCommand, IChecksumCommand checksumCommand)
    {
        _calcCommand = calcCommand;
        _checksumCommand = checksumCommand;
    }

    private Command GetCommand()
    {
        var features = new List<IFeature>()
        {
            _calcCommand,
            _checksumCommand
        };

        var command = new Command("hash",
            "Calculate hash and checksum fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32, CRC8, ...)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }
}