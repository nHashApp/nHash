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
        command.Subcommands.Add(GetJwtBuildCommand());
        command.Subcommands.Add(GetSemverCommand());
        command.Subcommands.Add(GetNumberCommand());

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

    private BaseCommand GetJwtBuildCommand()
    {
        var headerOption = new Option<string>("--header", "-H") { Description = "JSON header string", DefaultValueFactory = _ => "{\"alg\":\"HS256\",\"typ\":\"JWT\"}" };
        var payloadOption = new Option<string>("--payload", "-p") { Description = "JSON payload string", Required = true };

        var cmd = new BaseCommand("jwt-build", "Build an unsigned JWT token from JSON header and payload strings");
        cmd.Options.Add(headerOption);
        cmd.Options.Add(payloadOption);

        cmd.SetAction(parseResult =>
        {
            var header = parseResult.GetValue(headerOption) ?? "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
            var payload = parseResult.GetValue(payloadOption) ?? string.Empty;

            var res = devService.BuildJwt(header, payload);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }

    private BaseCommand GetSemverCommand()
    {
        var version1Arg = new Argument<string>("version1") { Description = "First semver string (e.g. 1.0.0-alpha)" };
        var version2Arg = new Argument<string>("version2") { Description = "Second semver string (e.g. 1.0.0)" };

        var cmd = new BaseCommand("semver", "Compare two semantic versions");
        cmd.Arguments.Add(version1Arg);
        cmd.Arguments.Add(version2Arg);

        cmd.SetAction(parseResult =>
        {
            var v1 = parseResult.GetValue(version1Arg) ?? string.Empty;
            var v2 = parseResult.GetValue(version2Arg) ?? string.Empty;

            var res = devService.CompareSemver(v1, v2);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }

    private BaseCommand GetNumberCommand()
    {
        var valueArg = new Argument<string>("value") { Description = "Number value to inspect" };

        var cmd = new BaseCommand("number", "Inspect a number and show its representations in decimal, hex, octal, binary, and scientific formats");
        cmd.Arguments.Add(valueArg);

        cmd.SetAction(parseResult =>
        {
            var val = parseResult.GetValue(valueArg) ?? string.Empty;

            var res = devService.InspectNumber(val);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }
}

