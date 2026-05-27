using nHash.Application.Encodes;
using nHash.Application.Encodes.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class JwtTokenCommand(IJwtTokenService jwtTokenService, IOutputProvider outputProvider) : IJwtTokenCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Option<bool> _noWriteInformation = new("--no-summary") { Description = "Don't write human readable information", DefaultValueFactory = _ => false };
    private readonly Argument<string> _textArgument = new("token") { Description = "Jwt token for decode" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("jwt", "JWT token decode (Comply with GDPR rules)", GetExamples());
        command.Options.Add(_noWriteInformation);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument);
            var noSummary = parseResult.GetValue(_noWriteInformation);
            DecodeJwtToken(text ?? string.Empty, noSummary);
        });
        command.Aliases.Add("j");

        return command;
    }
    
    private static List<KeyValuePair<string,string>> GetExamples() =>
        [
            new( "To decode a JWT token", "nhash encode jwt eyJhbGciOiJIUzI1NiIsInR5..."  ),
            new( "To decode a JWT token", "nhash e j eyJhbGciOiJIUzI1NiIsInR5..."  )
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