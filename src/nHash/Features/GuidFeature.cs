namespace nHash.Features;

public class GuidFeature : IFeature
{
    public Command Command => GetCommand();

    private readonly Option<bool> _withBracket = new(name: "--bracket", description: "Generate with brackets");
    private readonly Option<bool> _withoutHyphen = new(name: "--no-hyphen", description: "Generate without hyphens");

    private Command GetCommand()
    {
        var command = new Command("uuid", "Generate a Universally unique identifier (UUID/GUID)")
        {
            _withBracket,
            _withoutHyphen
        };
        command.SetHandler(GenerateUuid, _withBracket, _withoutHyphen);

        return command;
    }

    private static void GenerateUuid(bool withBracket, bool withoutHyphen)
    {
        var guid = Guid.NewGuid();
        var result = withBracket ? guid.ToString("B") : guid.ToString();
        if (withoutHyphen)
        {
            result = result.Replace("-", "");
        }

        Console.WriteLine(result);
    }
}