using nHash.Application.Texts.Models;

namespace nHash.Application.Texts;

public interface ITextToolsService
{
    string GenerateSlug(string text, string separator);
    WordFrequencyResult CountWordFrequency(string text, int topN);
    PalindromeResult CheckPalindrome(string text, bool ignoreCase, bool ignoreSpaces);
    OccurrencesResult CountOccurrences(string text, string pattern, bool useRegex);
    string EscapeString(string text, string targetLanguage, bool unescape);
}
