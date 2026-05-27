using System.Text.RegularExpressions;

namespace nHash.Application.Texts;

public class CaseConverterService : ICaseConverterService
{
    public string Convert(string text, string format)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        return format.ToLowerInvariant().Trim() switch
        {
            "camel" => ToCamelCase(text),
            "pascal" => ToPascalCase(text),
            "snake" => ToSnakeCase(text),
            "kebab" => ToKebabCase(text),
            "upper" => text.ToUpperInvariant(),
            "lower" => text.ToLowerInvariant(),
            _ => text
        };
    }

    private static string ToPascalCase(string text)
    {
        var words = Regex.Split(text, @"[^A-Za-z0-9]+");
        var sb = new System.Text.StringBuilder();
        foreach (var word in words)
        {
            if (word.Length == 0) continue;
            sb.Append(char.ToUpperInvariant(word[0]));
            if (word.Length > 1)
            {
                sb.Append(word[1..].ToLowerInvariant());
            }
        }
        return sb.ToString();
    }

    private static string ToCamelCase(string text)
    {
        var pascal = ToPascalCase(text);
        if (string.IsNullOrEmpty(pascal)) return string.Empty;
        return char.ToLowerInvariant(pascal[0]) + pascal[1..];
    }

    private static string ToSnakeCase(string text)
    {
        var words = Regex.Split(text, @"[^A-Za-z0-9]+");
        var cleanWords = words.Where(w => w.Length > 0).Select(w => w.ToLowerInvariant());
        return string.Join('_', cleanWords);
    }

    private static string ToKebabCase(string text)
    {
        var words = Regex.Split(text, @"[^A-Za-z0-9]+");
        var cleanWords = words.Where(w => w.Length > 0).Select(w => w.ToLowerInvariant());
        return string.Join('-', cleanWords);
    }
}
