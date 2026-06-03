using nHash.Application.Texts.Models;

namespace nHash.Application.Cryptos.Passwords;

public interface IPasswordService
{
    string GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix);
    string GeneratePassphrase(int wordCount, char separator);
    PasswordStrengthResult EvaluatePasswordStrength(string password);
}