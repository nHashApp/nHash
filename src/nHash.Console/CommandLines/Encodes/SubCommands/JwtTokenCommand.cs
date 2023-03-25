using nHash.Application.Encodes;
using nHash.Application.Encodes.Models;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class JwtTokenCommand : IJwtTokenCommand
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _noWriteInformation;
    private readonly Argument<string> _textArgument;

    private readonly IJwtTokenService _jwtTokenService;
    private readonly IOutputProvider _outputProvider;

    public JwtTokenCommand(IJwtTokenService jwtTokenService, IOutputProvider outputProvider)
    {
        _jwtTokenService = jwtTokenService;
        _outputProvider = outputProvider;

        _noWriteInformation = new Option<bool>(name: "--no-summary", () => false,
            description: "Don't write human readable information");
        _textArgument = new Argument<string>("token", "Jwt token for decode");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("jwt", "JWT token decode (Comply with GDPR rules)")
        {
            _noWriteInformation
        };
        command.AddArgument(_textArgument);
        command.SetHandler(DecodeJwtToken, _textArgument, _noWriteInformation);

        return command;
    }

    private void DecodeJwtToken(string text, bool noWriteInformation)
    {
       var jwtResult=  _jwtTokenService.DecodeJwtToken(text, noWriteInformation);
        
        _outputProvider.AppendLine();
        _outputProvider.AppendLine("Header: (ALGORITHM & TOKEN TYPE)");
        _outputProvider.AppendLine(jwtResult.Header);

        _outputProvider.AppendLine();
        _outputProvider.AppendLine("Payload: (DATA)");
        _outputProvider.AppendLine(jwtResult.Payload);

        if (jwtResult.Summary is null)
        {
            return;
        }

        _outputProvider.AppendLine();
        _outputProvider.AppendLine("Summary:");
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
            _outputProvider.AppendLine("    Algorithm: " + summary.Algorithm);
        }

    }

    private void WriteSummaryPayload(JwtTokenSummary summary)
    {
        if (!string.IsNullOrWhiteSpace(summary.Issuer))
        {
            _outputProvider.AppendLine("    Issuer: " + summary.Issuer);
        }

        if (summary.IssuedAt is not null)
        {
            _outputProvider.AppendLine("    Issued at: " + summary.IssuedAt);
        }

        if (!string.IsNullOrWhiteSpace(summary.Id))
        {
            _outputProvider.AppendLine("    Id: " + summary.Id);
        }

        if (!string.IsNullOrWhiteSpace(summary.Audience))
        {
            _outputProvider.AppendLine("    Audience: " + summary.Audience);
        }

        if (!string.IsNullOrWhiteSpace(summary.Subject))
        {
            _outputProvider.AppendLine("    Subject: " + summary.Subject);
        }

        if (summary.Expiration is not null)
        {
            _outputProvider.AppendLine("    Expiration: " + summary.Expiration);
        }

    }
}