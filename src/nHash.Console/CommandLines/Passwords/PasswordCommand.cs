using nHash.Application.Passwords;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Passwords;

public class PasswordCommand : IPasswordCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Option<bool> _upperCase;
    private readonly Option<bool> _lowerCase;
    private readonly Option<bool> _numeric;
    private readonly Option<bool> _specialChar;
    private readonly Option<string> _customChar;
    private readonly Option<int> _length;
    private readonly Option<string> _prefix;
    private readonly Option<string> _suffix;

    private readonly IPasswordService _passwordService;
    private readonly IOutputProvider _outputProvider;

    public PasswordCommand(IOutputProvider outputProvider, IPasswordService passwordService)
    {
        _outputProvider = outputProvider;
        _passwordService = passwordService;
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
        _length.AddAlias("-l");
        _prefix = new Option<string>(name: "--prefix", () => string.Empty, description: "Prefix");
        _suffix = new Option<string>(name: "--suffix", () => string.Empty, description: "Suffix");
    }

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("password",
            "Generate a random password with custom length, prefix, suffix, character, etc options", GetExamples())
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
        command.AddAlias("p");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples()
    {
        return new List<KeyValuePair<string, string>>()
        {
            new(
                "Random password with a length of 12 characters, uppercase letters, lowercase letters, and numbers",
                "nhash password -l 12 --no-special"),
            new("Random password with a length of 8 characters, only uppercase letters",
                "nhash password -l 8 --no-lower --no-number --no-special"),
            new("Random password with a custom character set and a length of 10 characters",
                "nhash password --custom abc123 -l 10"),
            new("Random password with a length of 20 characters and a prefix and suffix",
                "nhash password --length 20 --prefix \"nHash-\" --suffix \"-2023\""),
        };
    }

    private void GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix)
    {
        var returnText = _passwordService.GeneratePassword(noUpperCase, noLowerCase, noNumeric, noSpecialChar,
            customChar, length, prefix, suffix);
        _outputProvider.AppendLine(returnText);
    }
}