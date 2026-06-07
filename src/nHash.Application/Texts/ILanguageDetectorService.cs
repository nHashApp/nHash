using nHash.Application.Texts.Models;

namespace nHash.Application.Texts;

public interface ILanguageDetectorService
{
    LanguageResult DetectLanguage(string text);
}
