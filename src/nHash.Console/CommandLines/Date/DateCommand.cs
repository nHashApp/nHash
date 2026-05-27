using System.CommandLine;
using nHash.Application.Date;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Date;

public class DateCommand(IDateService dateService, IOutputProvider outputProvider) : IDateCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("date", "Date, time, epoch, and calendar utility subcommands");
        command.Aliases.Add("time");

        command.Subcommands.Add(GetEpochCommand());
        command.Subcommands.Add(GetConvertCommand());
        command.Subcommands.Add(GetDiffCommand());
        command.Subcommands.Add(GetTimezoneCommand());

        return command;
    }

    private BaseCommand GetEpochCommand()
    {
        var valueArg = new Argument<string>("value") { Description = "Unix epoch value or date-time string (if omitted, returns current epoch)", Arity = ArgumentArity.ZeroOrOne };
        var msOption = new Option<bool>("--ms", "-m") { Description = "Treat epoch value as milliseconds" };

        var cmd = new BaseCommand("epoch", "Convert Unix epoch to date-time or date-time to Unix epoch");
        cmd.Arguments.Add(valueArg);
        cmd.Options.Add(msOption);

        cmd.SetAction(parseResult =>
        {
            var value = parseResult.GetValue(valueArg);
            var ms = parseResult.GetValue(msOption);

            if (string.IsNullOrWhiteSpace(value))
            {
                var res = dateService.DateTimeToEpoch(string.Empty);
                outputProvider.AppendLine(res);
            }
            else if (long.TryParse(value, out long epochVal))
            {
                var res = dateService.EpochToDateTime(epochVal, ms);
                outputProvider.AppendLine(res);
            }
            else
            {
                var res = dateService.DateTimeToEpoch(value);
                outputProvider.AppendLine(res);
            }
        });

        return cmd;
    }

    private BaseCommand GetConvertCommand()
    {
        var dateArg = new Argument<string>("date") { Description = "Date-time string to convert (e.g. 2026-05-27 12:00:00 or 1402/03/06)" };
        var fromOption = new Option<string>("--from", "-f") { Description = "Source calendar type (gregorian, jalali/persian, hijri)", DefaultValueFactory = _ => "gregorian" };
        var toOption = new Option<string>("--to", "-t") { Description = "Target calendar type (gregorian, jalali/persian, hijri)", DefaultValueFactory = _ => "jalali" };

        var cmd = new BaseCommand("convert", "Convert dates between Gregorian, Jalali (Shamsi), and Hijri calendars");
        cmd.Arguments.Add(dateArg);
        cmd.Options.Add(fromOption);
        cmd.Options.Add(toOption);

        cmd.SetAction(parseResult =>
        {
            var date = parseResult.GetValue(dateArg) ?? string.Empty;
            var fromCal = parseResult.GetValue(fromOption) ?? "gregorian";
            var toCal = parseResult.GetValue(toOption) ?? "jalali";

            var res = dateService.ConvertCalendars(date, fromCal, toCal);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }

    private BaseCommand GetDiffCommand()
    {
        var startArg = new Argument<string>("start") { Description = "Start date-time string" };
        var endArg = new Argument<string>("end") { Description = "End date-time string" };

        var cmd = new BaseCommand("diff", "Calculate difference/duration between two dates");
        cmd.Arguments.Add(startArg);
        cmd.Arguments.Add(endArg);

        cmd.SetAction(parseResult =>
        {
            var start = parseResult.GetValue(startArg) ?? string.Empty;
            var end = parseResult.GetValue(endArg) ?? string.Empty;

            var res = dateService.CalculateDifference(start, end);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }

    private BaseCommand GetTimezoneCommand()
    {
        var dateArg = new Argument<string>("date") { Description = "Date-time string to convert" };
        var fromOption = new Option<string>("--from", "-f") { Description = "Source timezone ID (e.g. UTC, 'Iran Standard Time', 'Eastern Standard Time')", DefaultValueFactory = _ => "UTC" };
        var toOption = new Option<string>("--to", "-t") { Description = "Target timezone ID (e.g. 'Iran Standard Time', 'Greenwich Standard Time')", DefaultValueFactory = _ => "UTC" };

        var cmd = new BaseCommand("timezone", "Convert a date-time from one timezone to another");
        cmd.Arguments.Add(dateArg);
        cmd.Options.Add(fromOption);
        cmd.Options.Add(toOption);

        cmd.SetAction(parseResult =>
        {
            var date = parseResult.GetValue(dateArg) ?? string.Empty;
            var fromTz = parseResult.GetValue(fromOption) ?? "UTC";
            var toTz = parseResult.GetValue(toOption) ?? "UTC";

            var res = dateService.ConvertTimezone(date, fromTz, toTz);
            outputProvider.AppendLine(res);
        });

        return cmd;
    }
}
