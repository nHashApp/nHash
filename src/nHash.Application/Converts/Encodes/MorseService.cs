using System.Text;

namespace nHash.Application.Encodes;

public class MorseService : IMorseService
{
    private static readonly Dictionary<char, string> EncodeTable = new()
    {
        { 'A', ".-" },    { 'B', "-..." },  { 'C', "-.-." },
        { 'D', "-.." },   { 'E', "." },     { 'F', "..-." },
        { 'G', "--." },   { 'H', "...." },  { 'I', ".." },
        { 'J', ".---" },  { 'K', "-.-" },   { 'L', ".-.." },
        { 'M', "--" },    { 'N', "-." },    { 'O', "---" },
        { 'P', ".--." },  { 'Q', "--.-" },  { 'R', ".-." },
        { 'S', "..." },   { 'T', "-" },     { 'U', "..-" },
        { 'V', "...-" },  { 'W', ".--" },   { 'X', "-..-" },
        { 'Y', "-.--" },  { 'Z', "--.." },
        { '0', "-----" }, { '1', ".----" }, { '2', "..---" },
        { '3', "...--" }, { '4', "....-" }, { '5', "....." },
        { '6', "-...." }, { '7', "--..." }, { '8', "---.." },
        { '9', "----." },
        { '.', ".-.-.-" }, { ',', "--..--" }, { '?', "..--.." }, { '/', "-..-." },
    };

    private static readonly Dictionary<string, char> DecodeTable = BuildDecodeTable();

    private static Dictionary<string, char> BuildDecodeTable()
    {
        var table = new Dictionary<string, char>();
        foreach (var kvp in new Dictionary<char, string>
        {
            { 'A', ".-" },    { 'B', "-..." },  { 'C', "-.-." },
            { 'D', "-.." },   { 'E', "." },     { 'F', "..-." },
            { 'G', "--." },   { 'H', "...." },  { 'I', ".." },
            { 'J', ".---" },  { 'K', "-.-" },   { 'L', ".-.." },
            { 'M', "--" },    { 'N', "-." },    { 'O', "---" },
            { 'P', ".--." },  { 'Q', "--.-" },  { 'R', ".-." },
            { 'S', "..." },   { 'T', "-" },     { 'U', "..-" },
            { 'V', "...-" },  { 'W', ".--" },   { 'X', "-..-" },
            { 'Y', "-.--" },  { 'Z', "--.." },
            { '0', "-----" }, { '1', ".----" }, { '2', "..---" },
            { '3', "...--" }, { '4', "....-" }, { '5', "....." },
            { '6', "-...." }, { '7', "--..." }, { '8', "---.." },
            { '9', "----." },
            { '.', ".-.-.-" }, { ',', "--..--" }, { '?', "..--.." }, { '/', "-..-." },
        })
        {
            table[kvp.Value] = kvp.Key;
        }

        return table;
    }

    public string Calculate(string text, bool decode)
    {
        return decode ? Decode(text) : Encode(text);
    }

    private static string Encode(string text)
    {
        var words = text.ToUpperInvariant().Split(' ');
        var encodedWords = new List<string>();

        foreach (var word in words)
        {
            var letters = new List<string>();
            foreach (var ch in word)
            {
                if (EncodeTable.TryGetValue(ch, out var morse))
                    letters.Add(morse);
            }

            if (letters.Count > 0)
                encodedWords.Add(string.Join(" ", letters));
        }

        return string.Join(" / ", encodedWords);
    }

    private static string Decode(string morseText)
    {
        var result = new StringBuilder();
        var words = morseText.Split(new[] { " / " }, StringSplitOptions.None);

        foreach (var word in words)
        {
            if (result.Length > 0)
                result.Append(' ');

            var letters = word.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var letter in letters)
            {
                if (DecodeTable.TryGetValue(letter, out var ch))
                    result.Append(ch);
                else
                    result.Append('?');
            }
        }

        return result.ToString();
    }
}
