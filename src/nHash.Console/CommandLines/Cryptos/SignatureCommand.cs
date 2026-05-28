using System.CommandLine;
using nHash.Application.Cryptos;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Cryptos;

public class SignatureCommand(ISignatureService service, IOutputProvider outputProvider) : ISignatureCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("signature", "RSA digital signature utilities (keygen, sign, verify)", GetExamples());
        command.Aliases.Add("sig");

        command.Subcommands.Add(GetKeygenSubCommand());
        command.Subcommands.Add(GetSignSubCommand());
        command.Subcommands.Add(GetVerifySubCommand());

        return command;
    }

    private BaseCommand GetKeygenSubCommand()
    {
        var sizeOption = new Option<int>("--size", "-s") { Description = "RSA key size in bits (e.g. 2048, 4096)", DefaultValueFactory = _ => 2048 };

        var cmd = new BaseCommand("keygen", "Generate an RSA public/private key pair");
        cmd.Options.Add(sizeOption);

        cmd.SetAction(parseResult =>
        {
            var size = parseResult.GetValue(sizeOption);
            var result = service.GenerateKeyPair(size);
            outputProvider.AppendLine(result);
        });

        return cmd;
    }

    private BaseCommand GetSignSubCommand()
    {
        var dataArg = new Argument<string>("data") { Description = "Data string to sign" };
        var keyOption = new Option<string>("--key", "-k") { Description = "Path to the RSA private key PEM file", Required = true };

        var cmd = new BaseCommand("sign", "Sign data using an RSA private key (SHA-256)");
        cmd.Arguments.Add(dataArg);
        cmd.Options.Add(keyOption);

        cmd.SetAction(parseResult =>
        {
            var data = parseResult.GetValue(dataArg) ?? string.Empty;
            var keyPath = parseResult.GetValue(keyOption) ?? string.Empty;

            try
            {
                var pem = System.IO.File.ReadAllText(keyPath);
                var result = service.Sign(data, pem);
                outputProvider.AppendLine(result);
            }
            catch (Exception ex)
            {
                outputProvider.AppendLine($"Error reading key file: {ex.Message}");
            }
        });

        return cmd;
    }

    private BaseCommand GetVerifySubCommand()
    {
        var dataArg = new Argument<string>("data") { Description = "Data string that was signed" };
        var keyOption = new Option<string>("--key", "-k") { Description = "Path to the RSA public key PEM file", Required = true };
        var sigOption = new Option<string>("--signature", "-g") { Description = "Base64-encoded signature to verify", Required = true };

        var cmd = new BaseCommand("verify", "Verify a data signature using an RSA public key");
        cmd.Arguments.Add(dataArg);
        cmd.Options.Add(keyOption);
        cmd.Options.Add(sigOption);

        cmd.SetAction(parseResult =>
        {
            var data = parseResult.GetValue(dataArg) ?? string.Empty;
            var keyPath = parseResult.GetValue(keyOption) ?? string.Empty;
            var sig = parseResult.GetValue(sigOption) ?? string.Empty;

            try
            {
                var pem = System.IO.File.ReadAllText(keyPath);
                var result = service.Verify(data, sig, pem);
                outputProvider.AppendLine(result);
            }
            catch (Exception ex)
            {
                outputProvider.AppendLine($"Error reading key file: {ex.Message}");
            }
        });

        return cmd;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
    [
        new("Generate a 2048-bit RSA key pair", "nhash crypto signature keygen"),
        new("Generate a 4096-bit RSA key pair", "nhash crypto signature keygen -s 4096"),
        new("Sign data with a private key", "nhash crypto signature sign \"hello world\" -k private.pem"),
        new("Verify a signature with a public key", "nhash crypto signature verify \"hello world\" -k public.pem -g <base64sig>"),
    ];
}
