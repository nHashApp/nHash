using nHash.Application.Passwords;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Passwords;

public class PasswordCommand(IOutputProvider outputProvider, IPasswordService passwordService) : IPasswordCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Option<bool> _upperCase = new("--no-upper") { Description = "Include uppercase Characters (A-Z) or not" };
    private readonly Option<bool> _lowerCase = new("--no-lower") { Description = "Include lowercase Characters (a-z) or not" };
    private readonly Option<bool> _numeric = new("--no-number") { Description = "Include numbers (1234567890) or not" };
    private readonly Option<bool> _specialChar = new("--no-special") { Description = "Include symbols (*$-+?_&=!%{}/) or not" };
    private readonly Option<string> _customChar = new("--custom") { Description = "Custom characters. If select the custom character other options was removed", DefaultValueFactory = _ => string.Empty };
    private readonly Option<int> _length = new("--length", "-l") { Description = "Password length", DefaultValueFactory = _ => 16 };
    private readonly Option<string> _prefix = new("--prefix") { Description = "Prefix", DefaultValueFactory = _ => string.Empty };
    private readonly Option<string> _suffix = new("--suffix") { Description = "Suffix", DefaultValueFactory = _ => string.Empty };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("password",
            "Generate a random password with custom length, prefix, suffix, character, etc options", GetExamples());

        command.Options.Add(_upperCase);
        command.Options.Add(_lowerCase);
        command.Options.Add(_numeric);
        command.Options.Add(_specialChar);
        command.Options.Add(_customChar);
        command.Options.Add(_length);
        command.Options.Add(_prefix);
        command.Options.Add(_suffix);

        command.SetAction(parseResult =>
        {
            var noUpper = parseResult.GetValue(_upperCase);
            var noLower = parseResult.GetValue(_lowerCase);
            var noNum = parseResult.GetValue(_numeric);
            var noSpecial = parseResult.GetValue(_specialChar);
            var custom = parseResult.GetValue(_customChar);
            var len = parseResult.GetValue(_length);
            var pref = parseResult.GetValue(_prefix);
            var suff = parseResult.GetValue(_suffix);
            GeneratePassword(noUpper, noLower, noNum, noSpecial, custom ?? string.Empty, len, pref ?? string.Empty, suff ?? string.Empty);
        });

        command.Aliases.Add("p");

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new(
                "Random password with a length of 12 characters, uppercase letters, lowercase letters, and numbers",
                "nhash password -l 12 --no-special"),
            new("Random password with a length of 8 characters, only uppercase letters",
                "nhash password -l 8 --no-lower --no-number --no-special"),
            new("Random password with a custom character set and a length of 10 characters",
                "nhash password --custom abc123 -l 10"),
            new("Random password with a length of 20 characters and a prefix and suffix",
                "nhash password --length 20 --prefix \"nHash-\" --suffix \"-2023\""),
        ];

    private void GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix)
    {
        var returnText = passwordService.GeneratePassword(noUpperCase, noLowerCase, noNumeric, noSpecialChar,
            customChar, length, prefix, suffix);
        outputProvider.AppendLine(returnText);
    }
}