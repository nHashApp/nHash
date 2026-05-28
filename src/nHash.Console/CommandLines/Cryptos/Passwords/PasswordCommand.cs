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
    private readonly Option<bool> _passphrase = new("--passphrase") { Description = "Generate a readable passphrase instead of a random password" };
    private readonly Option<int> _words = new("--words") { Description = "Number of words for passphrase", DefaultValueFactory = _ => 4 };
    private readonly Option<string> _separator = new("--separator") { Description = "Separator character between passphrase words", DefaultValueFactory = _ => "-" };
    private readonly Option<string> _estimate = new("--estimate") { Description = "Evaluate and estimate the strength of the provided password" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("password",
            "Generate a random password/passphrase or estimate password strength", GetExamples());

        command.Options.Add(_upperCase);
        command.Options.Add(_lowerCase);
        command.Options.Add(_numeric);
        command.Options.Add(_specialChar);
        command.Options.Add(_customChar);
        command.Options.Add(_length);
        command.Options.Add(_prefix);
        command.Options.Add(_suffix);
        command.Options.Add(_passphrase);
        command.Options.Add(_words);
        command.Options.Add(_separator);
        command.Options.Add(_estimate);

        command.SetAction(parseResult =>
        {
            var estimate = parseResult.GetValue(_estimate);
            if (!string.IsNullOrWhiteSpace(estimate))
            {
                var estimateResult = passwordService.EvaluatePasswordStrength(estimate);
                outputProvider.AppendLine(estimateResult);
                return;
            }

            var passphrase = parseResult.GetValue(_passphrase);
            if (passphrase)
            {
                var words = parseResult.GetValue(_words);
                var sep = parseResult.GetValue(_separator);
                char sepChar = '-';
                if (!string.IsNullOrEmpty(sep))
                {
                    sepChar = sep[0];
                }
                var phrase = passwordService.GeneratePassphrase(words, sepChar);
                outputProvider.AppendLine(phrase);
                return;
            }

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
            new("Random password with a length of 12 characters", "nhash password -l 12 --no-special"),
            new("Generate a readable 4-word passphrase", "nhash password --passphrase --words 4"),
            new("Evaluate and estimate password strength", "nhash password --estimate \"mySecurePassword123!\""),
        ];

    private void GeneratePassword(bool noUpperCase, bool noLowerCase, bool noNumeric, bool noSpecialChar,
        string customChar, int length, string prefix, string suffix)
    {
        var returnText = passwordService.GeneratePassword(noUpperCase, noLowerCase, noNumeric, noSpecialChar,
            customChar, length, prefix, suffix);
        outputProvider.AppendLine(returnText);
    }
}