using nHash.Application.Texts.Models;

namespace nHash.Application.Texts;

public interface ITextDiffService
{
    TextDiffResult Compare(string text1, string text2);
}
