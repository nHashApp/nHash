using System.CommandLine;
using System.Linq;
using nHash.Application.Ids;
using nHash.Application.Ids.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Ids;

public class UuidInspectCommand(IUuidInspectService service, IOutputProvider outputProvider) : IUuidInspectCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private BaseCommand GetFeatureCommand()
    {
        var uuidArg = new Argument<string>("uuid") { Description = "UUID string to inspect" };

        var command = new BaseCommand("inspect", "Inspect a UUID and extract version, variant, timestamp and byte layout", GetExamples());
        command.Arguments.Add(uuidArg);
        command.Aliases.Add("ins");

        command.SetAction(parseResult =>
        {
            var uuid = parseResult.GetValue(uuidArg) ?? string.Empty;
            var result = service.Inspect(uuid);
            if (!result.Success)
            {
                outputProvider.AppendLine(result.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"UUID:     {result.ParsedGuid}");
            outputProvider.AppendLine($"Version:  {result.Version}");
            outputProvider.AppendLine($"Variant:  {result.Variant}");

            if (result.Version == 1)
            {
                if (result.Timestamp.HasValue)
                {
                    outputProvider.AppendLine($"Timestamp:{result.Timestamp.Value:yyyy-MM-dd HH:mm:ss.fffffff} {result.TimestampDescription}");
                }
                if (result.ClockSequence.HasValue)
                {
                    outputProvider.AppendLine($"Clock Seq: {result.ClockSequence.Value}");
                }
            }
            else if (result.Version == 4)
            {
                outputProvider.AppendLine($"Timestamp: {result.TimestampDescription}");
                outputProvider.AppendLine("Random:    True (randomly generated UUID)");
            }
            else if (result.Version == 7)
            {
                if (result.Timestamp.HasValue)
                {
                    outputProvider.AppendLine($"Timestamp: {result.Timestamp.Value:yyyy-MM-dd HH:mm:ss.fff} {result.TimestampDescription}");
                }
                outputProvider.AppendLine($"Unix ms:   {result.UnixMs}");
            }
            else
            {
                outputProvider.AppendLine($"Timestamp: {result.TimestampDescription}");
            }

            var hexBytes = string.Join(' ', result.RfcBytes.Select(b => b.ToString("X2")));
            outputProvider.AppendLine($"Bytes:    {hexBytes}");
        });

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
    [
        new("Inspect a UUID v4", "nhash id inspect 550e8400-e29b-41d4-a716-446655440000"),
        new("Inspect a UUID v7 (time-ordered)", "nhash id inspect 018f4e3e-7bc4-7000-8000-000000000001"),
        new("Inspect using alias", "nhash id ins 550e8400-e29b-41d4-a716-446655440000"),
    ];
}
