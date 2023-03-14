using System.Text;
using MlkPwgen;

namespace nHash.Providers.Text;

public class PassGenerator : IFeature
{
    public Command Command => GetFeatureCommand();


    private readonly Option<bool> _upperCase;
    private readonly Option<bool> _lowerCase;
    private readonly Option<bool> _numeric;
    private readonly Option<bool> _specialChar;
    private readonly Option<string> _customChar;
    private readonly Option<int> _length;
    private readonly Option<string> _prefix;
    private readonly Option<string> _suffix;

    private const string CharsLCase = "abcdefgijkmnopqrstwxyz";
    private const string CharsUCase = "ABCDEFGHJKLMNPQRSTWXYZ";
    private const string CharsNumeric = "0123456789";
    private const string CharsSpecial = "*$-+?_&=!%{}/";

    public PassGenerator()
    {
        _upperCase = new Option<bool>(name: "--no-upper", () => false,
            description: "Include uppercase Characters (A-Z) or not");
        _lowerCase = new Option<bool>(name: "--no-lower", () => false,
            description: "Include lowercase Characters (a-z) or not");
        _numeric = new Option<bool>(name: "--no-number", () => false,
            description: "Include numbers (1234567890) or not");
        _specialChar = new Option<bool>(name: "--no-special", () => false,
            description: "Include symbols (*$-+?_&=!%{}/) or not");
        _customChar = new Option<string>(name: "--custom", () => string.Empty,
            description: "Custom characters. If select the custom character other options was removed");
        _length = new Option<int>(name: "--length", () => 16, description: "Password length");
        _prefix = new Option<string>(name: "--prefix", () => string.Empty, description: "Prefix");
        _suffix = new Option<string>(name: "--suffix", () => string.Empty, description: "Suffix");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("password",
            "Generate a random password with custom length, prefix, suffix, character, etc options")
        {
            _upperCase,
            _lowerCase,
            _numeric,
            _specialChar,
            _customChar,
            _length,
            _prefix,
            _suffix,
        };
        command.SetHandler(GeneratePassword, _upperCase, _lowerCase, _numeric, _specialChar, _customChar, _length,
            _prefix, _suffix);

        return command;
    }

    private static void GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix)
    {
        var pass = GetPassword(noUpperCase, noLowerCase, noNumeric, noSpecialChar, customChar, length, prefix, suffix);
        Console.WriteLine(pass);
    }

    private static string GetPassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
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