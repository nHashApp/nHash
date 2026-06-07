using System.Reflection;
using FastText.NetWrapper;
using nHash.Application.Texts.Models;

namespace nHash.Application.Texts;

public class LanguageDetectorService : ILanguageDetectorService
{
    private FastTextWrapper? _fastText;
    private readonly object _lock = new();

    private FastTextWrapper GetFastText()
    {
        if (_fastText != null)
        {
            return _fastText;
        }

        lock (_lock)
        {
            if (_fastText != null)
            {
                return _fastText;
            }

            var assembly = typeof(LanguageDetectorService).Assembly;
            using var stream = assembly.GetManifestResourceStream("nHash.Application.Texts.Resources.lid.176.ftz");
            if (stream == null)
            {
                throw new InvalidOperationException("Embedded model file 'lid.176.ftz' not found in nHash.Application assembly resources.");
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var modelBytes = ms.ToArray();

            var fastText = new FastTextWrapper();
            fastText.LoadModel(modelBytes);
            _fastText = fastText;
            return _fastText;
        }
    }

    public LanguageResult DetectLanguage(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return new LanguageResult
            {
                Success = false,
                ErrorMessage = "Text cannot be empty."
            };
        }

        try
        {
            var fastText = GetFastText();
            var prediction = fastText.PredictSingle(text);
            if (string.IsNullOrEmpty(prediction.Label))
            {
                return new LanguageResult
                {
                    Success = false,
                    ErrorMessage = "Failed to detect language."
                };
            }

            var languageCode = prediction.Label.Replace("__label__", "");
            return new LanguageResult
            {
                Success = true,
                LanguageCode = languageCode,
                Confidence = prediction.Probability
            };
        }
        catch (Exception ex)
        {
            return new LanguageResult
            {
                Success = false,
                ErrorMessage = $"An error occurred during language detection: {ex.Message}"
            };
        }
    }
}
