using System.CommandLine;
using nHash.Application.Texts;
using nHash.Application.Abstraction;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Texts.SubCommands;

public class PalindromeCommand(ITextToolsService textToolsService, IOutputProvider outputProvider) : IPalindromeCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _textArgument = new("text") { Description = "Text to check for palindrome" };
    private readonly Option<bool> _ignoreCaseOption = new("--ignore-case", "-i") { Description = "Ignore character casing", DefaultValueFactory = _ => true };
    private readonly Option<bool> _ignoreSpacesOption = new("--ignore-spaces", "-w") { Description = "Ignore spaces and punctuation", DefaultValueFactory = _ => true };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("palindrome", "Check if text is a palindrome", GetExamples());
        command.Arguments.Add(_textArgument);
        command.Options.Add(_ignoreCaseOption);
        command.Options.Add(_ignoreSpacesOption);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument) ?? string.Empty;
            var ignoreCase = parseResult.GetValue(_ignoreCaseOption);
            var ignoreSpaces = parseResult.GetValue(_ignoreSpacesOption);
            var result = textToolsService.CheckPalindrome(text, ignoreCase, ignoreSpaces);
            outputProvider.AppendLine(result);
        });
        command.Aliases.Add("pal");
        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Check palindrome", "nhash text palindrome \"racecar\""),
            new("Check palindrome with punctuation", "nhash text palindrome \"A man, a plan, a canal: Panama!\"")
        ];
}
