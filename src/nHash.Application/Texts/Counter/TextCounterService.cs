namespace nHash.Application.Texts.Counter;

public class TextCounterService
{
    public static int WordCount(string text)
    {
        int wordCount = 0;
        bool isWord = false;

        // Iterate through each character in the string
        foreach (char c in text)
        {
            // If the character is a letter, set isWord to true
            if (char.IsLetter(c))
            {
                isWord = true;
            }
            // If the character is not a letter and isWord is true, increment the word count
            else if (isWord)
            {
                wordCount++;
                isWord = false;
            }
        }

        // If the last character was a letter, increment the word count
        if (isWord)
        {
            wordCount++;
        }

        return wordCount;
    }
    
    public static void CountCharactersWordsSentences(string text)
    {
        // Count number of characters
        var characterCount = text.Length;
    
        // Count number of words
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
    
        // Count number of sentences
        var sentenceCount = 0;
        foreach (var c in text)
        {
            if (c is '.' or '?' or '!')
            {
                sentenceCount++;
            }
        }
    
        // Output results
        Console.WriteLine("Character count: {0}", characterCount);
        Console.WriteLine("Word count: {0}", wordCount);
        Console.WriteLine("Sentence count: {0}", sentenceCount);
    }
}