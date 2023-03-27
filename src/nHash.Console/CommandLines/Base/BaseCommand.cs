namespace nHash.Console.CommandLines.Base;

public class BaseCommand : Command
{
    public List<KeyValuePair<string,string>>? Examples { get; }

    public BaseCommand(string name, string? description = null, List<KeyValuePair<string,string>>? examples = null) : base(name,
        description)
    {
        Examples = examples;
    }
}