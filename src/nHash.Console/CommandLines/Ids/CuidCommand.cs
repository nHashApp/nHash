using nHash.Application.Ids;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Ids;

public class CuidCommand(ICuidService cuidService, IOutputProvider outputProvider) : ICuidCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<int> _length = new("--length", "-l") { Description = "Desired length of CUID2 (default 24)", DefaultValueFactory = _ => 24 };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("cuid", "Generate a secure, random, URL-safe CUID2", GetExamples());
        command.Options.Add(_length);
        command.SetAction(parseResult =>
        {
            var len = parseResult.GetValue(_length);
            GenerateId(len);
        });
        command.Aliases.Add("ci");

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Generate a CUID2", "nhash id cuid"),
            new("Generate a CUID2 with length 16", "nhash id cuid -l 16"),
        ];

    private void GenerateId(int length)
    {
        var id = cuidService.Generate(length);
        outputProvider.AppendLine(id);
    }
}
