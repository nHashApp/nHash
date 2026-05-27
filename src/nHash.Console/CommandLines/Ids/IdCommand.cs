using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Uuids;

namespace nHash.Console.CommandLines.Ids;

public class IdCommand(
    IUuidCommand uuidFeature,
    ISnowflakeCommand snowflakeFeature,
    ICuidCommand cuidFeature)
    : IIdCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        List<IFeature> features =
        [
            uuidFeature,
            snowflakeFeature,
            cuidFeature
        ];

        var command = new BaseCommand("id", "Unique Identifier utilities (UUID, Snowflake, CUID2)");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}
