using nHash.Application.Encodes;
using nHash.Application.Encodes.Models;
using nHash.Application.Dev;
using nHash.Console.CommandLines.Base;
using System.CommandLine;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class JwtTokenCommand(IJwtTokenService jwtTokenService, IDevService devService, IOutputProvider outputProvider) : IJwtTokenCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("jwt", "JWT token tools (decode, build)", GetExamples());
        command.Aliases.Add("j");

        command.Subcommands.Add(GetDecodeSubCommand());
        command.Subcommands.Add(GetBuildSubCommand());

        return command;
    }

    private BaseCommand GetDecodeSubCommand()
    {
        var noWriteInformation = new Option<bool>("--no-summary") { Description = "Don't write human readable information", DefaultValueFactory = _ => false };
        var textArgument = new Argument<string>("token") { Description = "Jwt token for decode" };

        var cmd = new BaseCommand("decode", "Decode a JWT token and display its header, payload, and summary info");
        cmd.Aliases.Add("d");
        cmd.Aliases.Add("dec");
        cmd.Options.Add(noWriteInformation);
        cmd.Arguments.Add(textArgument);

        cmd.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(textArgument) ?? string.Empty;
            var noSummary = parseResult.GetValue(noWriteInformation);
            DecodeJwtToken(text, noSummary);
        });

        return cmd;
    }

    private BaseCommand GetBuildSubCommand()
    {
        var headerOption = new Option<string>("--header", "-H") { Description = "JSON header string", DefaultValueFactory = _ => "{\"alg\":\"HS256\",\"typ\":\"JWT\"}" };
        var payloadOption = new Option<string>("--payload", "-p") { Description = "JSON payload string", Required = true };

        var cmd = new BaseCommand("build", "Build an unsigned JWT token from JSON header and payload strings");
        cmd.Aliases.Add("b");
        cmd.Aliases.Add("gen");
        cmd.Aliases.Add("create");
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

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Decode a JWT token", "nhash encode jwt decode eyJhbGciOiJIUzI1NiIsInR5..."),
            new("Decode using short alias", "nhash e j d eyJhbGciOiJIUzI1NiIsInR5..."),
            new("Build a JWT token", "nhash encode jwt build -p \"{\\\"sub\\\":\\\"123\\\"}\""),
            new("Build using short alias", "nhash e j b -p \"{\\\"sub\\\":\\\"123\\\"}\"")
        ];

    private void DecodeJwtToken(string text, bool noWriteInformation)
    {
        var jwtResult = jwtTokenService.DecodeJwtToken(text, noWriteInformation);
         
        outputProvider.AppendLine();
        outputProvider.AppendLine("Header: (ALGORITHM & TOKEN TYPE)");
        outputProvider.AppendLine(jwtResult.Header);

        outputProvider.AppendLine();
        outputProvider.AppendLine("Payload: (DATA)");
        outputProvider.AppendLine(jwtResult.Payload);

        if (jwtResult.Summary is null)
        {
            return;
        }

        outputProvider.AppendLine();
        outputProvider.AppendLine("Summary:");
        WriteSummary(jwtResult.Summary!);
    }

    private void WriteSummary(JwtTokenSummary summary)
    {
        WriteSummaryHeader(summary);
        WriteSummaryPayload(summary);
    }
    
    private void WriteSummaryHeader(JwtTokenSummary summary)
    {
        if (!string.IsNullOrWhiteSpace(summary.Algorithm))
        {
            outputProvider.AppendLine("    Algorithm: " + summary.Algorithm);
        }
    }

    private void WriteSummaryPayload(JwtTokenSummary summary)
    {
        if (!string.IsNullOrWhiteSpace(summary.Issuer))
        {
            outputProvider.AppendLine("    Issuer: " + summary.Issuer);
        }

        if (summary.IssuedAt is not null)
        {
            outputProvider.AppendLine("    Issued at: " + summary.IssuedAt);
        }

        if (!string.IsNullOrWhiteSpace(summary.Id))
        {
            outputProvider.AppendLine("    Id: " + summary.Id);
        }

        if (!string.IsNullOrWhiteSpace(summary.Audience))
        {
            outputProvider.AppendLine("    Audience: " + summary.Audience);
        }

        if (!string.IsNullOrWhiteSpace(summary.Subject))
        {
            outputProvider.AppendLine("    Subject: " + summary.Subject);
        }

        if (summary.Expiration is not null)
        {
            outputProvider.AppendLine("    Expiration: " + summary.Expiration);
        }
    }
}