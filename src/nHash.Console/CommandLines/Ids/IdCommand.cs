using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Uuids;

namespace nHash.Console.CommandLines.Ids;

public class IdCommand(
    IUuidCommand uuidFeature,
    ISnowflakeCommand snowflakeFeature,
    ICuidCommand cuidFeature,
    IUuidInspectCommand uuidInspectFeature,
    ITotpCommand totpFeature)
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
            totpFeature
        ];

        var command = new BaseCommand("id", "Unique Identifier utilities (UUID, Snowflake, CUID2, UUID Inspect, TOTP)");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}

