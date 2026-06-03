namespace nHash.Application.Texts.Counter;

public class TextCounterService
{
    public static int WordCount(string text)
    {
        int wordCount = 0;
        bool isWord = false;

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                isWord = true;
            }
            else if (isWord)
            {
                wordCount++;
                isWord = false;
            }
        }

        if (isWord)
        {
            wordCount++;
        }

        return wordCount;
    }

    public static (int Characters, int Words, int Sentences) CountCharactersWordsSentences(string text)
    {
        var characterCount = text.Length;

        var wordCount = 0;
        var isWord = false;
        foreach (var c in text)
        {
            if (char.IsWhiteSpace(c) || char.IsPunctuation(c))
            {
                if (isWord)
                {
                    wordCount++;
                    isWord = false;
                }
            }
            else
            {
                isWord = true;
            }
        }
        if (isWord)
        {
            wordCount++;
        }

        var sentenceCount = 0;
        foreach (var c in text)
        {
            if (c is '.' or '?' or '!')
            {
                sentenceCount++;
            }
        }

        return (characterCount, wordCount, sentenceCount);
    }
}
