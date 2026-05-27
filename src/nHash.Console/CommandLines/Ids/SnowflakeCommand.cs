using nHash.Application.Ids;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Ids;

public class SnowflakeCommand(ISnowflakeService snowflakeService, IOutputProvider outputProvider) : ISnowflakeCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<int> _workerId = new("--worker", "-w") { Description = "Machine/Worker ID (0-1023)", DefaultValueFactory = _ => 1 };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("snowflake", "Generate a 64-bit time-ordered Snowflake ID", GetExamples());
        command.Options.Add(_workerId);
        command.SetAction(parseResult =>
        {
            var worker = parseResult.GetValue(_workerId);
            GenerateId(worker);
        });
        command.Aliases.Add("sf");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Generate a Snowflake ID", "nhash id snowflake"),
            new("Generate a Snowflake ID with worker ID 12", "nhash id snowflake -w 12"),
        ];

    private void GenerateId(int workerId)
    {
        var id = snowflakeService.Generate(workerId);
        outputProvider.AppendLine(id.ToString());
    }
}
