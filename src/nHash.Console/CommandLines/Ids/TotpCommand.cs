using System.CommandLine;
using nHash.Application.Ids;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Ids;

public class TotpCommand(ITotpService service, IOutputProvider outputProvider) : ITotpCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private BaseCommand GetFeatureCommand()
    {
        var secretArg = new Argument<string>("secret") { Description = "Base32-encoded TOTP secret key" };
        var digitsOption = new Option<int>("--digits", "-d") { Description = "Number of OTP digits", DefaultValueFactory = _ => 6 };
        var periodOption = new Option<int>("--period", "-p") { Description = "TOTP period in seconds", DefaultValueFactory = _ => 30 };

        var command = new BaseCommand("totp", "Generate a Time-based One-Time Password (TOTP/HOTP) code per RFC 6238", GetExamples());
        command.Arguments.Add(secretArg);
        command.Options.Add(digitsOption);
        command.Options.Add(periodOption);
        command.Aliases.Add("otp");

        command.SetAction(parseResult =>
        {
            var secret = parseResult.GetValue(secretArg) ?? string.Empty;
            var digits = parseResult.GetValue(digitsOption);
            var period = parseResult.GetValue(periodOption);

            var result = service.Generate(secret, digits, period);
            outputProvider.AppendLine(result);
        });

        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
    [
        new("Generate a 6-digit TOTP (30s period)", "nhash id totp JBSWY3DPEHPK3PXP"),
        new("Generate an 8-digit TOTP with 60s period", "nhash id totp JBSWY3DPEHPK3PXP -d 8 -p 60"),
        new("Using alias", "nhash id otp JBSWY3DPEHPK3PXP"),
    ];
}
