using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using nHash.Application.Texts.Models;

namespace nHash.Application.Texts;

public class TextStatisticsService : ITextStatisticsService
{
    public TextStatisticsResult Calculate(string text)
    {
        var result = new TextStatisticsResult();
        if (string.IsNullOrEmpty(text))
        {
            result.Success = false;
            result.ErrorMessage = "Text is empty";
            return result;
        }

        result.CharactersCountWithSpaces = text.Length;
        result.CharactersCountNoSpaces = text.Count(c => !char.IsWhiteSpace(c));
        
        var lines = text.Split(["\r\n", "\n"], StringSplitOptions.None);
        result.LinesCount = lines.Length;

        result.WordsCount = Regex.Matches(text, @"\b\w+\b").Count;

        var paragraphs = text.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries);
        result.ParagraphsCount = paragraphs.Length;

        result.BytesCountUtf8 = Encoding.UTF8.GetByteCount(text);
        result.BytesCountUtf16 = Encoding.Unicode.GetByteCount(text);
        result.BytesCountAscii = Encoding.ASCII.GetByteCount(text);

        result.Success = true;
        return result;
    }
}
