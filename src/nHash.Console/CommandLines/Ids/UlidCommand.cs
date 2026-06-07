using nHash.Application.Ids;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Ids;

public class UlidCommand(IUlidService ulidService, IOutputProvider outputProvider) : IUlidCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Option<int> _countOption = new("--count", "-c") { Description = "Number of ULIDs to generate", DefaultValueFactory = _ => 1 };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("ulid", "Universally Unique Lexicographically Sortable Identifier", GetExamples());
        command.Options.Add(_countOption);

        command.SetAction(parseResult =>
        {
            var count = parseResult.GetValue(_countOption);
            var res = ulidService.Generate(count);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            foreach (var ulid in res.Ulids)
            {
                outputProvider.AppendLine(ulid);
            }
        });

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Generate one ULID", "nhash id ulid"),
            new("Generate 5 ULIDs", "nhash id ulid -c 5")
        ];
}
