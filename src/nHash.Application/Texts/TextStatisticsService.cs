using System.Text;
using System.Text.RegularExpressions;

namespace nHash.Application.Texts;

public class TextStatisticsService : ITextStatisticsService
{
    public string Calculate(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return "Text is empty";
        }

        int charCountWithSpaces = text.Length;
        int charCountWithoutSpaces = text.Count(c => !char.IsWhiteSpace(c));
        
        var lines = text.Split(["\r\n", "\n"], StringSplitOptions.None);
        int lineCount = lines.Length;

        int wordCount = Regex.Matches(text, @"\b\w+\b").Count;

        var paragraphs = text.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries);
        int paragraphCount = paragraphs.Length;

        int byteCountUtf8 = Encoding.UTF8.GetByteCount(text);
        int byteCountUtf16 = Encoding.Unicode.GetByteCount(text);
        int byteCountAscii = Encoding.ASCII.GetByteCount(text);

        var sb = new StringBuilder();
        sb.AppendLine($"Lines: {lineCount}");
        sb.AppendLine($"Paragraphs: {paragraphCount}");
        sb.AppendLine($"Words: {wordCount}");
        sb.AppendLine($"Characters (with spaces): {charCountWithSpaces}");
        sb.AppendLine($"Characters (no spaces): {charCountWithoutSpaces}");
        sb.AppendLine($"Bytes (UTF-8): {byteCountUtf8}");
        sb.AppendLine($"Bytes (UTF-16/Unicode): {byteCountUtf16}");
        sb.AppendLine($"Bytes (ASCII): {byteCountAscii}");

        return sb.ToString();
    }
}
