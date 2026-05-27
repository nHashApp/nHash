using System.CommandLine;
using nHash.Application.Dev;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Dev;

public class DevCommand(IDevService devService, IOutputProvider outputProvider) : IDevCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("dev", "Developer utility subcommands (cron, regex, color)");
        command.Aliases.Add("developer");

        command.Subcommands.Add(GetCronCommand());
        command.Subcommands.Add(GetRegexCommand());
        command.Subcommands.Add(GetColorCommand());

        return command;
    }

    private BaseCommand GetCronCommand()
    {
        var exprArg = new Argument<string>("expression") { Description = "Cron expression to parse (e.g. '*/5 12 * * 1-5')" };
        var countOption = new Option<int>("--count", "-c") { Description = "Number of next execution times to list", DefaultValueFactory = _ => 5 };

        var cmd = new BaseCommand("cron", "Translate a cron expression to human-readable text and estimate schedule");
        cmd.Arguments.Add(exprArg);
        cmd.Options.Add(countOption);

        cmd.SetAction(parseResult =>
        {
            var expr = parseResult.GetValue(exprArg) ?? string.Empty;
            var count = parseResult.GetValue(countOption);

            var res = devService.ParseCron(expr, count);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }

    private BaseCommand GetRegexCommand()
    {
        var inputArg = new Argument<string>("input") { Description = "Input text to run regex against" };
        var patternOption = new Option<string>("--pattern", "-p") { Description = "Regular expression pattern", Required = true };

        var cmd = new BaseCommand("regex", "Test regular expressions against an input string and show matches");
        cmd.Arguments.Add(inputArg);
        cmd.Options.Add(patternOption);

        cmd.SetAction(parseResult =>
        {
            var input = parseResult.GetValue(inputArg) ?? string.Empty;
            var pattern = parseResult.GetValue(patternOption) ?? string.Empty;

            var res = devService.TestRegex(pattern, input);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }

    private BaseCommand GetColorCommand()
    {
        var valArg = new Argument<string>("value") { Description = "Color value (hex like '#FF5733' or RGB like '255,87,51')" };

        var cmd = new BaseCommand("color", "Convert between color spaces (HEX, RGB, HSL, CMYK) with terminal preview");
        cmd.Arguments.Add(valArg);

        cmd.SetAction(parseResult =>
        {
            var val = parseResult.GetValue(valArg) ?? string.Empty;
            var res = devService.ConvertColor(val);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }
}
