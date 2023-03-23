using nHash.Application.Passwords;

namespace nHash.Console.CommandLines.Passwords;

public class PasswordCommand : IPasswordCommand
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

    private void GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix)
    {
        _passwordService.GeneratePassword(noUpperCase, noLowerCase, noNumeric, noSpecialChar, customChar, length,
            prefix, suffix);
    }
}