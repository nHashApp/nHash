namespace nHash.Application.Passwords;

public interface IPasswordTools
{
    string Generate(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix);
}