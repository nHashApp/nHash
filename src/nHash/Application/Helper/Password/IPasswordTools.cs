namespace nHash.Application.Helper.Password;

public interface IPasswordTools
{
    string Generate(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix);
}