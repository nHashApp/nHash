namespace nHash.Application.Texts.Counter;

public interface ITextCounterService
{
    int WordCount(string text);
    void CountCharactersWordsSentences(string text);
}
