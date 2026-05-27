using System.Text;
using MlkPwgen;

namespace nHash.Application.Passwords;

public class PasswordService : IPasswordService
{

    private const string CharsLCase = "abcdefgijkmnopqrstwxyz";
    private const string CharsUCase = "ABCDEFGHJKLMNPQRSTWXYZ";
    private const string CharsNumeric = "0123456789";
    private const string CharsSpecial = "*$-+?_&=!%{}/";
    
    public string GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix)
    {
        var pass = Generate(noUpperCase, noLowerCase, noNumeric, noSpecialChar, customChar, length,
            prefix, suffix);
        return pass;
    }
    
    private static string Generate(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix)
    {
        var pLen = !string.IsNullOrWhiteSpace(prefix) ? prefix.Length : 0;
        var sLen = !string.IsNullOrWhiteSpace(suffix) ? suffix.Length : 0;
        if (pLen + sLen > length)
        {
            return prefix + suffix;
        }

        var len = length - pLen - sLen;
        var passStr = GetRawPasswordString(noUpperCase, noLowerCase, noNumeric, noSpecialChar, customChar);

        if (string.IsNullOrWhiteSpace(passStr))
        {
            return string.Empty;
        }

        var res = PasswordGenerator.Generate(len, passStr);
        res = $"{prefix}{res}{suffix}";
        return res;
    }

    private static string GetRawPasswordString(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar)
    {
        if (!string.IsNullOrWhiteSpace(customChar))
        {
            return customChar;
        }

        var passStr = new StringBuilder();

        if (!noLowerCase)
        {
            passStr.Append(CharsLCase);
        }

        if (!noUpperCase)
        {
            passStr.Append(CharsUCase);
        }

        if (!noNumeric)
        {
            passStr.Append(CharsNumeric);
        }

        if (!noSpecialChar)
        {
            passStr.Append(CharsSpecial);
        }

        return passStr.ToString();
    }

    private static readonly string[] WordList = [
        "active", "agent", "alpha", "amber", "angel", "animal", "apple", "apron", "aqua", "arrow",
        "artist", "atomic", "autumn", "baby", "bacon", "badge", "banana", "beacon", "beauty", "berry",
        "bird", "black", "blaze", "blue", "bold", "bottle", "brass", "brave", "breeze", "brick",
        "bright", "bronze", "brown", "brush", "bubble", "butter", "cabin", "cable", "camel", "camera",
        "candle", "canyon", "canvas", "carbon", "castle", "cavity", "celery", "cement", "center", "chain",
        "chair", "chalk", "charm", "cherry", "chief", "chimney", "chorus", "chrome", "cider", "cigar",
        "cinema", "circle", "circus", "citrus", "clay", "cliff", "climb", "clock", "cloud", "clover",
        "coal", "coast", "cobalt", "coffee", "coin", "cold", "colony", "color", "comet", "cookie",
        "copper", "coral", "corner", "cosmic", "cotton", "county", "cousin", "cradle", "craft", "crater",
        "crayon", "cream", "creek", "crest", "cricket", "crown", "crystal", "cube", "cuckoo", "cupcake",
        "curry", "curtain", "custom", "cycle", "cyclone", "cynic", "dagger", "daily", "dairy", "daisy",
        "danger", "daring", "dark", "darling", "dawn", "dazzle", "dealer", "decade", "decimal", "deck",
        "decor", "decoy", "deep", "deer", "defend", "define", "degree", "delay", "delta", "deluge",
        "demise", "denim", "dental", "depot", "depth", "deputy", "derby", "desert", "design", "desire",
        "detail", "detect", "device", "devote", "diary", "dice", "diet", "differ", "digit", "dilemma",
        "dinner", "dinosaur", "direct", "dirt", "disaster", "disc", "discuss", "disease", "dish", "dismiss",
        "display", "distance", "divert", "divide", "divine", "dizzy", "doctor", "dodge", "dogma", "dollar"
    ];

    public string GeneratePassphrase(int wordCount, char separator)
    {
        if (wordCount < 1) wordCount = 4;
        
        var words = new string[wordCount];
        var bytes = new byte[wordCount * 4];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        for (int i = 0; i < wordCount; i++)
        {
            uint index = BitConverter.ToUInt32(bytes, i * 4) % (uint)WordList.Length;
            words[i] = WordList[index];
        }

        return string.Join(separator, words);
    }

    public string EvaluatePasswordStrength(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return "Password is empty";
        }

        int length = password.Length;
        int poolSize = 0;

        bool hasLower = false;
        bool hasUpper = false;
        bool hasDigit = false;
        bool hasSpecial = false;

        foreach (var c in password)
        {
            if (char.IsLower(c)) hasLower = true;
            else if (char.IsUpper(c)) hasUpper = true;
            else if (char.IsDigit(c)) hasDigit = true;
            else hasSpecial = true;
        }

        if (hasLower) poolSize += 26;
        if (hasUpper) poolSize += 26;
        if (hasDigit) poolSize += 10;
        if (hasSpecial) poolSize += 33;

        if (poolSize == 0) poolSize = 1;

        double entropy = length * Math.Log2(poolSize);
        
        string strength = entropy switch
        {
            < 28 => "Very Weak (Instantly crackable)",
            < 36 => "Weak (Crackable in minutes/hours)",
            < 60 => "Medium (Crackable in days/months)",
            < 80 => "Strong (Highly secure)",
            _ => "Very Strong (Virtually uncrackable)"
        };

        var sb = new StringBuilder();
        sb.AppendLine($"Password Length: {length}");
        sb.AppendLine($"Character Pool Size: {poolSize} (Lower={hasLower}, Upper={hasUpper}, Digit={hasDigit}, Special={hasSpecial})");
        sb.AppendLine($"Entropy: {entropy:F2} bits");
        sb.AppendLine($"Strength: {strength}");

        double totalCombinations = Math.Pow(poolSize, length);
        double hashesPerSec = 100_000_000_000.0;
        double secondsToCrack = totalCombinations / hashesPerSec;

        string crackTime;
        if (secondsToCrack < 1) crackTime = "Instant";
        else if (secondsToCrack < 60) crackTime = $"{secondsToCrack:F0} seconds";
        else if (secondsToCrack < 3600) crackTime = $"{secondsToCrack / 60:F0} minutes";
        else if (secondsToCrack < 86400) crackTime = $"{secondsToCrack / 3600:F0} hours";
        else if (secondsToCrack < 31536000) crackTime = $"{secondsToCrack / 86400:F0} days";
        else if (secondsToCrack < 31536000000) crackTime = $"{secondsToCrack / 31536000:F0} years";
        else crackTime = "Centuries / Forever";

        sb.AppendLine($"Estimated Cracking Time (at 100B hashes/sec): {crackTime}");

        return sb.ToString();
    }
}