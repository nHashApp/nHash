using System;
using System.CommandLine;
using System.Collections.Generic;
using nHash.Application.Cryptos.Hashes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Cryptos.Hashes.SubCommands;

public class Pbkdf2Command(IPbkdf2Service pbkdf2Service, IOutputProvider outputProvider) : IPbkdf2Command
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Argument<string> _passwordArgument = new("password") { Description = "Password to derive key from" };
    private readonly Argument<string> _saltArgument = new("salt") { Description = "Salt value" };
    private readonly Option<int> _iterationsOption = new("--iterations", "-i") { Description = "Number of iterations", DefaultValueFactory = _ => 10000 };
    private readonly Option<string> _algorithmOption = new("--algorithm", "-a") { Description = "Hash algorithm to use (sha1, sha256, sha384, sha512)", DefaultValueFactory = _ => "sha256" };
    private readonly Option<int> _lengthOption = new("--length", "-l") { Description = "Output key length in bytes", DefaultValueFactory = _ => 32 };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("pbkdf2", "Derive key from a password using PBKDF2 (Rfc2898)", GetExamples());
        command.Arguments.Add(_passwordArgument);
        command.Arguments.Add(_saltArgument);
        command.Options.Add(_iterationsOption);
        command.Options.Add(_algorithmOption);
        command.Options.Add(_lengthOption);

        command.SetAction(parseResult =>
        {
            var password = parseResult.GetValue(_passwordArgument) ?? string.Empty;
            var salt = parseResult.GetValue(_saltArgument) ?? string.Empty;
            var iterations = parseResult.GetValue(_iterationsOption);
            var algorithm = parseResult.GetValue(_algorithmOption) ?? "sha256";
            var length = parseResult.GetValue(_lengthOption);

            var res = pbkdf2Service.DeriveKey(password, salt, iterations, algorithm, length);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine("PBKDF2 Derived Key:");
            outputProvider.AppendLine($"Hex:    {res.DerivedKeyHex}");
            outputProvider.AppendLine($"Base64: {res.DerivedKeyBase64}");
        });

        command.Aliases.Add("pbk");
        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Derive a 256-bit key (32 bytes)", "nhash crypto hash pbkdf2 \"myPassword\" \"mySaltValue\""),
            new("Derive a key with 100k iterations and SHA-512", "nhash crypto hash pbkdf2 \"myPassword\" \"mySaltValue\" -i 100000 -a sha512 -l 64")
        ];
}
