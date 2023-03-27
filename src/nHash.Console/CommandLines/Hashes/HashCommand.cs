using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Hashes.SubCommands;

namespace nHash.Console.CommandLines.Hashes;

public class HashCommand : IHashCommand
{
    public BaseCommand Command => GetCommand();

    private readonly ICalcCommand _calcCommand;
    private readonly IChecksumCommand _checksumCommand;

    public HashCommand(ICalcCommand calcCommand, IChecksumCommand checksumCommand)
    {
        _calcCommand = calcCommand;
        _checksumCommand = checksumCommand;
    }

    private BaseCommand GetCommand()
    {
        var features = new List<IFeature>()
        {
            _calcCommand,
            _checksumCommand
        };

        var command = new BaseCommand("hash",
            "Calculate hash and checksum fingerprint (MD5, SHA-1, SHA-256, SHA-384, SHA-512, CRC32, CRC8, ...)",
            GetExamples());
        command.AddAlias("h");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples()
    {
        return new List<KeyValuePair<string, string>>()
        {
            new("Calc", "nhash hash calc <text> [options]"),
            new("Checksum", "nhash hash checksum <text> [options]"),
            new("Checksum", "nhash h ch <text> [options]"),
        };
    }
}