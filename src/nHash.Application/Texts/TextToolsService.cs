using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace nHash.Application.Texts;

public class TextToolsService : ITextToolsService
{
    public string GenerateSlug(string text, string separator)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        if (string.IsNullOrEmpty(separator)) separator = "-";

        // Normalize to NFKD to decompose diacritics
        var normalized = text.Normalize(NormalizationForm.FormKD);

        // Remove non-spacing marks (diacritics)
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        var result = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

        // Replace non-alphanumeric characters with separator
        var sepPattern = Regex.Escape(separator);
        result = Regex.Replace(result, @"[^a-z0-9]+", separator);

        // Collapse multiple separators
        result = Regex.Replace(result, $"{sepPattern}+", separator);

        // Trim separators from edges
        result = result.Trim(separator.ToCharArray());

        return result;
    }

    public string CountWordFrequency(string text, int topN)
    {
        if (string.IsNullOrWhiteSpace(text)) return "No text provided.";

        // Split by whitespace and punctuation
        var words = Regex.Split(text, @"[\s\p{P}]+")
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Select(w => w.ToLowerInvariant())
            .ToArray();

        if (words.Length == 0) return "No words found.";

        var freq = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var word in words)
        {
            if (freq.TryGetValue(word, out var count))
                freq[word] = count + 1;
            else
                freq[word] = 1;
        }

        var top = freq
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key)
            .Take(topN > 0 ? topN : 10)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine($"Total words: {words.Length}, Unique words: {freq.Count}");
        sb.AppendLine();
        sb.AppendLine($"Top {top.Count} words:");
        foreach (var kv in top)
        {
            sb.AppendLine($"  {kv.Key}: {kv.Value}");
        }

        return sb.ToString().TrimEnd();
    }

    public string CheckPalindrome(string text, bool ignoreCase, bool ignoreSpaces)
    {
        if (string.IsNullOrEmpty(text)) return "No text provided.";

        var processed = text;
        if (ignoreSpaces)
            processed = processed.Replace(" ", string.Empty);
        if (ignoreCase)
            processed = processed.ToLowerInvariant();

        var reversed = new string(processed.Reverse().ToArray());
        var isPalindrome = processed == reversed;

        var sb = new StringBuilder();
        sb.AppendLine($"Input:    \"{text}\"");
        if (ignoreSpaces || ignoreCase)
            sb.AppendLine($"Processed:\"{processed}\"");
        sb.AppendLine($"Result:   {(isPalindrome ? "✓ IS a palindrome" : "✗ NOT a palindrome")}");

        return sb.ToString().TrimEnd();
    }

    public string CountOccurrences(string text, string pattern, bool useRegex)
    {
        if (string.IsNullOrEmpty(text)) return "No text provided.";
        if (string.IsNullOrEmpty(pattern)) return "No pattern provided.";

        var sb = new StringBuilder();

        if (useRegex)
        {
            try
            {
                var regex = new Regex(pattern, RegexOptions.None);
                var matches = regex.Matches(text);
                sb.AppendLine($"Pattern: \"{pattern}\" (regex)");
                sb.AppendLine($"Count:   {matches.Count}");
                if (matches.Count > 0)
                {
                    sb.AppendLine("Positions:");
                    foreach (Match m in matches)
                        sb.AppendLine($"  [{m.Index}..{m.Index + m.Length - 1}] => \"{m.Value}\"");
                }
            }
            catch (RegexParseException ex)
            {
                return $"Invalid regex: {ex.Message}";
            }
        }
        else
        {
            var count = 0;
            var positions = new List<int>();
            var idx = 0;
            while ((idx = text.IndexOf(pattern, idx, StringComparison.Ordinal)) >= 0)
            {
                positions.Add(idx);
                count++;
                idx += pattern.Length;
            }

            sb.AppendLine($"Pattern: \"{pattern}\" (literal)");
            sb.AppendLine($"Count:   {count}");
            if (positions.Count > 0)
            {
                sb.AppendLine("Positions:");
                foreach (var pos in positions)
                    sb.AppendLine($"  [{pos}..{pos + pattern.Length - 1}]");
            }
        }

        return sb.ToString().TrimEnd();
    }

    public string EscapeString(string text, string targetLanguage, bool unescape)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        var lang = targetLanguage.ToLowerInvariant().Trim();

        if (unescape)
        {
            return lang switch
            {
                "json" or "csharp" => UnescapeJsonCSharp(text),
                "sql" => text.Replace("''", "'"),
                "xml" => UnescapeXml(text),
                _ => UnescapeJsonCSharp(text)
            };
        }
        else
        {
            return lang switch
            {
                "json" => EscapeJson(text),
                "csharp" => EscapeCSharp(text),
                "sql" => text.Replace("'", "''"),
                "xml" => EscapeXml(text),
                _ => EscapeJson(text)
            };
        }
    }

    private static string EscapeJson(string text)
    {
        var sb = new StringBuilder();
        foreach (var c in text)
        {
            switch (c)
            {
                case '"': sb.Append("\\\""); break;
                case '\\': sb.Append("\\\\"); break;
                case '\n': sb.Append("\\n"); break;
                case '\r': sb.Append("\\r"); break;
                case '\t': sb.Append("\\t"); break;
                case '\b': sb.Append("\\b"); break;
                case '\f': sb.Append("\\f"); break;
                default:
                    if (c < 0x20)
                        sb.Append($"\\u{(int)c:x4}");
                    else
                        sb.Append(c);
                    break;
            }
        }
        return sb.ToString();
    }

    private static string EscapeCSharp(string text)
    {
        // Same as JSON but also escape @
        var sb = new StringBuilder();
        foreach (var c in text)
        {
            switch (c)
            {
                case '"': sb.Append("\\\""); break;
                case '\\': sb.Append("\\\\"); break;
                case '\n': sb.Append("\\n"); break;
                case '\r': sb.Append("\\r"); break;
                case '\t': sb.Append("\\t"); break;
                case '\0': sb.Append("\\0"); break;
                case '\a': sb.Append("\\a"); break;
                case '\b': sb.Append("\\b"); break;
                case '\f': sb.Append("\\f"); break;
                case '\v': sb.Append("\\v"); break;
                default: sb.Append(c); break;
            }
        }
        return sb.ToString();
    }

    private static string EscapeXml(string text)
    {
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
    }

    private static string UnescapeJsonCSharp(string text)
    {
        var sb = new StringBuilder();
        var i = 0;
        while (i < text.Length)
        {
            if (text[i] == '\\' && i + 1 < text.Length)
            {
                var next = text[i + 1];
                switch (next)
                {
                    case '"': sb.Append('"'); i += 2; break;
                    case '\\': sb.Append('\\'); i += 2; break;
                    case 'n': sb.Append('\n'); i += 2; break;
                    case 'r': sb.Append('\r'); i += 2; break;
                    case 't': sb.Append('\t'); i += 2; break;
                    case 'b': sb.Append('\b'); i += 2; break;
                    case 'f': sb.Append('\f'); i += 2; break;
                    case '0': sb.Append('\0'); i += 2; break;
                    case 'a': sb.Append('\a'); i += 2; break;
                    case 'v': sb.Append('\v'); i += 2; break;
                    case 'u' when i + 5 < text.Length:
                        var hex = text.Substring(i + 2, 4);
                        if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var code))
                        {
                            sb.Append((char)code);
                            i += 6;
                        }
                        else
                        {
                            sb.Append(text[i]);
                            i++;
                        }
                        break;
                    default:
                        sb.Append(text[i]);
                        i++;
                        break;
                }
            }
            else
            {
                sb.Append(text[i]);
                i++;
            }
        }
        return sb.ToString();
    }

    private static string UnescapeXml(string text)
    {
        return text
            .Replace("&amp;", "&")
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("&quot;", "\"")
            .Replace("&apos;", "'");
    }
}
