namespace nHash.Console.CommandLines.Base;

public class BaseCommand : Command
{
    public List<KeyValuePair<string,string>>? Examples { get; }

    public BaseCommand(string name, string? description = null, List<KeyValuePair<string,string>>? examples = null) : base(name,
        FormatDescription(description, examples))
    {
        Examples = examples;
    }

    private static string? FormatDescription(string? description, List<KeyValuePair<string, string>>? examples)
    {
        if (examples == null || examples.Count == 0)
        {
            return description;
        }

        var sb = new System.Text.StringBuilder();
        if (!string.IsNullOrEmpty(description))
        {
            sb.AppendLine(description);
            sb.AppendLine();
        }

        sb.AppendLine("Examples:");
        foreach (var example in examples)
        {
            sb.AppendLine($"  {example.Key}:");
            sb.AppendLine($"    {example.Value}");
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }
}