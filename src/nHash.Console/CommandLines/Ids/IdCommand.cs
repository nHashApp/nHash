using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Ids;

public class IdCommand(
    IUuidCommand uuidFeature,
    ISnowflakeCommand snowflakeFeature,
    ICuidCommand cuidFeature,
    IUuidInspectCommand uuidInspectFeature,
    ITotpCommand totpFeature,
    IUlidCommand ulidFeature,
    INanoIdCommand nanoidFeature)
    : IIdCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        List<IFeature> features =
        [
            uuidFeature,
            snowflakeFeature,
            cuidFeature,
            uuidInspectFeature,
            totpFeature,
            ulidFeature,
            nanoidFeature
        ];

        var command = new BaseCommand("id", "Unique Identifier utilities (UUID, Snowflake, CUID2, UUID Inspect, TOTP, ULID, NanoID)");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}

