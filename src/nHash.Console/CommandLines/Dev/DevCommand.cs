using System.CommandLine;
using nHash.Application.Dev;
using nHash.Application.Dev.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Dev;

public class DevCommand(IDevService devService, IOutputProvider outputProvider) : IDevCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("dev", "Developer utility subcommands (cron, regex, color, semver, number)");
        command.Aliases.Add("developer");

        command.Subcommands.Add(GetCronCommand());
        command.Subcommands.Add(GetRegexCommand());
        command.Subcommands.Add(GetColorCommand());
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
        cmd.Aliases.Add("cr");

        cmd.SetAction(parseResult =>
        {
            var expr = parseResult.GetValue(exprArg) ?? string.Empty;
            var count = parseResult.GetValue(countOption);

            var res = devService.ParseCron(expr, count);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine("Cron Description:");
            outputProvider.AppendLine($"- Minutes: {res.MinuteDescription}");
            outputProvider.AppendLine($"- Hours: {res.HourDescription}");
            outputProvider.AppendLine($"- Days of Month: {res.DayOfMonthDescription}");
            outputProvider.AppendLine($"- Months: {res.MonthDescription}");
            outputProvider.AppendLine($"- Days of Week: {res.DayOfWeekDescription}");
            outputProvider.AppendLine();
            outputProvider.AppendLine($"Next {count} Executions (Local Time):");
            if (res.NextExecutions.Count == 0)
            {
                outputProvider.AppendLine("  No matching execution times found in the near future.");
            }
            else
            {
                foreach (var time in res.NextExecutions)
                {
                    outputProvider.AppendLine($"  - {time:yyyy-MM-dd HH:mm:ss}");
                }
            }
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
        cmd.Aliases.Add("rg");

        cmd.SetAction(parseResult =>
        {
            var input = parseResult.GetValue(inputArg) ?? string.Empty;
            var pattern = parseResult.GetValue(patternOption) ?? string.Empty;

            var res = devService.TestRegex(pattern, input);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"Regex Pattern: {res.Pattern}");
            outputProvider.AppendLine($"Input Text: {res.Input}");
            outputProvider.AppendLine($"Is Match: {res.IsMatch}");
            outputProvider.AppendLine($"Matches Count: {res.MatchCount}");
            outputProvider.AppendLine();

            foreach (var match in res.Matches)
            {
                outputProvider.AppendLine($"Match #{match.MatchNumber}:");
                outputProvider.AppendLine($"  - Value: \"{match.Value}\"");
                outputProvider.AppendLine($"  - Index: {match.Index}");
                outputProvider.AppendLine($"  - Length: {match.Length}");

                if (match.CaptureGroups.Count > 0)
                {
                    outputProvider.AppendLine("  - Capture Groups:");
                    foreach (var group in match.CaptureGroups)
                    {
                        outputProvider.AppendLine($"    Group {group.Index} ({group.Name}): \"{group.Value}\" (Index: {group.Index})");
                    }
                }
                outputProvider.AppendLine();
            }
        });

        return cmd;
    }

    private BaseCommand GetColorCommand()
    {
        var valArg = new Argument<string>("value") { Description = "Color value (hex like '#FF5733' or RGB like '255,87,51')" };

        var cmd = new BaseCommand("color", "Convert between color spaces (HEX, RGB, HSL, CMYK) with terminal preview");
        cmd.Arguments.Add(valArg);
        cmd.Aliases.Add("c");

        cmd.SetAction(parseResult =>
        {
            var val = parseResult.GetValue(valArg) ?? string.Empty;
            var res = devService.ConvertColor(val);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            var colorBlock = $"\x1b[48;2;{res.R};{res.G};{res.B}m      \x1b[0m";
            outputProvider.AppendLine("Color Details:");
            outputProvider.AppendLine($"- Visual Preview: {colorBlock}");
            outputProvider.AppendLine($"- HEX: {res.Hex}");
            outputProvider.AppendLine($"- RGB: {res.Rgb}");
            outputProvider.AppendLine($"- HSL: {res.Hsl}");
            outputProvider.AppendLine($"- CMYK: {res.Cmyk}");
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
        cmd.Aliases.Add("sv");

        cmd.SetAction(parseResult =>
        {
            var v1 = parseResult.GetValue(version1Arg) ?? string.Empty;
            var v2 = parseResult.GetValue(version2Arg) ?? string.Empty;

            var res = devService.CompareSemver(v1, v2);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"Version 1: {res.Version1}");
            outputProvider.AppendLine($"  Major:      {res.Details1.Major}");
            outputProvider.AppendLine($"  Minor:      {res.Details1.Minor}");
            outputProvider.AppendLine($"  Patch:      {res.Details1.Patch}");
            if (!string.IsNullOrEmpty(res.Details1.Prerelease))
                outputProvider.AppendLine($"  Pre-release:{res.Details1.Prerelease}");
            if (!string.IsNullOrEmpty(res.Details1.Build))
                outputProvider.AppendLine($"  Build:      {res.Details1.Build}");
            outputProvider.AppendLine();
            outputProvider.AppendLine($"Version 2: {res.Version2}");
            outputProvider.AppendLine($"  Major:      {res.Details2.Major}");
            outputProvider.AppendLine($"  Minor:      {res.Details2.Minor}");
            outputProvider.AppendLine($"  Patch:      {res.Details2.Patch}");
            if (!string.IsNullOrEmpty(res.Details2.Prerelease))
                outputProvider.AppendLine($"  Pre-release:{res.Details2.Prerelease}");
            if (!string.IsNullOrEmpty(res.Details2.Build))
                outputProvider.AppendLine($"  Build:      {res.Details2.Build}");
            outputProvider.AppendLine();
            outputProvider.AppendLine($"Result:    {res.ComparisonResult}");
        });

        return cmd;
    }

    private BaseCommand GetNumberCommand()
    {
        var valueArg = new Argument<string>("value") { Description = "Number value to inspect" };

        var cmd = new BaseCommand("number", "Inspect a number and show its representations in decimal, hex, octal, binary, and scientific formats");
        cmd.Arguments.Add(valueArg);
        cmd.Aliases.Add("num");
        cmd.Aliases.Add("n");

        cmd.SetAction(parseResult =>
        {
            var val = parseResult.GetValue(valueArg) ?? string.Empty;

            var res = devService.InspectNumber(val);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"Input:       {res.Input}");
            outputProvider.AppendLine($"Type:        {res.Type}");

            if (res.IsInteger)
            {
                outputProvider.AppendLine($"Decimal:     {res.Decimal}");
                outputProvider.AppendLine($"Binary:      {res.Binary}");
                outputProvider.AppendLine($"Octal:       {res.Octal}");
                outputProvider.AppendLine($"Hexadecimal: {res.Hexadecimal}");
                outputProvider.AppendLine($"Scientific:  {res.Scientific}");
                outputProvider.AppendLine($"Positive:    {res.Positive}");
                outputProvider.AppendLine($"Even:        {res.Even}");
            }
            else
            {
                outputProvider.AppendLine($"Decimal:     {res.Decimal}");
                outputProvider.AppendLine($"Scientific:  {res.Scientific}");
                outputProvider.AppendLine($"Hex (raw):   {res.Hexadecimal}");
                outputProvider.AppendLine($"IsNaN:       {res.IsNaN}");
                outputProvider.AppendLine($"IsInfinity:  {res.IsInfinity}");
                outputProvider.AppendLine($"IsFinite:    {res.IsFinite}");
            }
        });

        return cmd;
    }
}
