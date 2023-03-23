using System.Text;
using MlkPwgen;
using nHash.Application.Abstraction;

namespace nHash.Application.Passwords;

public class PasswordService : IPasswordService
{
    private readonly IOutputProvider _outputProvider;

    private const string CharsLCase = "abcdefgijkmnopqrstwxyz";
    private const string CharsUCase = "ABCDEFGHJKLMNPQRSTWXYZ";
    private const string CharsNumeric = "0123456789";
    private const string CharsSpecial = "*$-+?_&=!%{}/";
    
    public PasswordService(IOutputProvider outputProvider)
    {
        _outputProvider = outputProvider;
    }

    public void GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix)
    {
        var pass = Generate(noUpperCase, noLowerCase, noNumeric, noSpecialChar, customChar, length,
            prefix, suffix);
        _outputProvider.Append(pass);
    }
    
    private string Generate(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
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
}