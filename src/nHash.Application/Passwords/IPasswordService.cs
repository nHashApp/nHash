namespace nHash.Application.Passwords;

public interface IPasswordService
{
    void GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix);
}