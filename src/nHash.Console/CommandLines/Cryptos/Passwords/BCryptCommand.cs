using System;
using System.CommandLine;
using System.Collections.Generic;
using nHash.Application.Cryptos.Passwords;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Cryptos.Passwords;

public class BCryptCommand(IBCryptService bcryptService, IOutputProvider outputProvider) : IBCryptCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("bcrypt", "BCrypt password hashing and verification");
        command.Subcommands.Add(GetHashCommand());
        command.Subcommands.Add(GetVerifyCommand());
        command.Aliases.Add("bc");
        return command;
    }

    private BaseCommand GetHashCommand()
    {
        var passwordArg = new Argument<string>("password") { Description = "Password to hash" };
        var workFactorOption = new Option<int>("--work-factor", "-w") { Description = "Work factor (4-31)", DefaultValueFactory = _ => 11 };

        var cmd = new BaseCommand("hash", "Hash a password using BCrypt", GetHashExamples());
        cmd.Arguments.Add(passwordArg);
        cmd.Options.Add(workFactorOption);

        cmd.SetAction(parseResult =>
        {
            var password = parseResult.GetValue(passwordArg) ?? string.Empty;
            var workFactor = parseResult.GetValue(workFactorOption);
            var res = bcryptService.Hash(password, workFactor);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }
            outputProvider.AppendLine(res.Hash);
        });

        return cmd;
    }

    private BaseCommand GetVerifyCommand()
    {
        var passwordArg = new Argument<string>("password") { Description = "Plain text password" };
        var hashOption = new Option<string>("--hash", "-h") { Description = "BCrypt hash to verify against", Required = true };

        var cmd = new BaseCommand("verify", "Verify a password against a BCrypt hash", GetVerifyExamples());
        cmd.Arguments.Add(passwordArg);
        cmd.Options.Add(hashOption);

        cmd.SetAction(parseResult =>
        {
            var password = parseResult.GetValue(passwordArg) ?? string.Empty;
            var hash = parseResult.GetValue(hashOption) ?? string.Empty;
            var res = bcryptService.Verify(password, hash);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }
            outputProvider.AppendLine(res.IsValid ? "Valid (PASS)" : "Invalid (FAIL)");
        });

        return cmd;
    }

    private static List<KeyValuePair<string, string>> GetHashExamples() =>
        [
            new("Hash a password", "nhash crypto password bcrypt hash \"myPassword123\""),
            new("Hash with custom work factor", "nhash crypto password bcrypt hash \"myPassword123\" -w 12")
        ];

    private static List<KeyValuePair<string, string>> GetVerifyExamples() =>
        [
            new("Verify a password", "nhash crypto password bcrypt verify \"myPassword123\" -h \"$2a$11$...\"")
        ];
}
