namespace nHash.Application.Texts;

public interface ITextToolsService
{
    string GenerateSlug(string text, string separator);
    string CountWordFrequency(string text, int topN);
    string CheckPalindrome(string text, bool ignoreCase, bool ignoreSpaces);
    string CountOccurrences(string text, string pattern, bool useRegex);
    string EscapeString(string text, string targetLanguage, bool unescape);
}
