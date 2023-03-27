using nHash.Application.Encodes;
using nHash.Application.Encodes.Models;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Encodes.SubCommands;

public class JwtTokenCommand : IJwtTokenCommand
{
    public BaseCommand Command => GetFeatureCommand();
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

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("jwt", "JWT token decode (Comply with GDPR rules)", GetExamples())
        {
            _noWriteInformation
        };
        command.AddArgument(_textArgument);
        command.SetHandler(DecodeJwtToken, _textArgument, _noWriteInformation);
        command.AddAlias("j");

        return command;
    }
    
    private static List<KeyValuePair<string,string>> GetExamples()
    {
        return new List<KeyValuePair<string,string>>()
        {
            new( "To decode a JWT token", "nhash encode jwt eyJhbGciOiJIUzI1NiIsInR5..."  ),
            new( "To decode a JWT token", "nhash e j eyJhbGciOiJIUzI1NiIsInR5..."  )
        };
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