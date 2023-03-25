namespace nHash.Application.Passwords;

public interface IPasswordService
{
    string GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix);
}