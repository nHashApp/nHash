namespace nHash.Application.Texts.Models;

public class WordFrequencyDetail
{
    public string Word { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class WordFrequencyResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int TotalWords { get; set; }
    public int UniqueWords { get; set; }
    public List<WordFrequencyDetail> TopWords { get; set; } = new();
}

public class TextStatisticsResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int LinesCount { get; set; }
    public int ParagraphsCount { get; set; }
    public int WordsCount { get; set; }
    public int CharactersCountWithSpaces { get; set; }
    public int CharactersCountNoSpaces { get; set; }
    public int BytesCountUtf8 { get; set; }
    public int BytesCountUtf16 { get; set; }
    public int BytesCountAscii { get; set; }
}

public enum DiffLineType
{
    Unchanged,
    Added,
    Removed
}

public class TextDiffLine
{
    public DiffLineType Type { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class TextDiffResult
{
    public List<TextDiffLine> DiffLines { get; set; } = new();
}

public class PasswordStrengthResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int Length { get; set; }
    public int PoolSize { get; set; }
    public bool HasLower { get; set; }
    public bool HasUpper { get; set; }
    public bool HasDigit { get; set; }
    public bool HasSpecial { get; set; }
    public double Entropy { get; set; }
    public string StrengthLabel { get; set; } = string.Empty;
    public string EstimatedCrackTime { get; set; } = string.Empty;
}

public class PalindromeResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
    public string Processed { get; set; } = string.Empty;
    public bool IsPalindrome { get; set; }
}

public class OccurrencePosition
{
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public string Value { get; set; } = string.Empty;
}

public class OccurrencesResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public bool IsRegex { get; set; }
    public int Count { get; set; }
    public List<OccurrencePosition> Positions { get; set; } = new();
}

public class LanguageResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
    public double Confidence { get; set; }
}
