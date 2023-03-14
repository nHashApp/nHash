using System.IdentityModel.Tokens.Jwt;

namespace nHash.Providers.Decodes;

public class JwtTokenDecodeFeature : IFeature
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _noWriteInformation;
    private readonly Argument<string> _textArgument;

    public JwtTokenDecodeFeature()
    {
        _noWriteInformation = new Option<bool>(name: "--no-info", () => false,
            description: "Don't write human readable information");
        _textArgument = new Argument<string>("token", "Jwt token for decode");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command("jwt", "JWT token decode")
        {
            _noWriteInformation
        };
        command.AddArgument(_textArgument);
        command.SetHandler(DecodeJwtToken, _textArgument, _noWriteInformation);

        return command;
    }

    private static void DecodeJwtToken(string text, bool noWriteInformation)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.ReadJwtToken(text);

        
        Console.WriteLine();
        Console.WriteLine("Header: (ALGORITHM & TOKEN TYPE)");
        WriteHeaders(jwt);

        Console.WriteLine();
        Console.WriteLine("Payload: (DATA)");
        WritePayload(jwt);

        if (noWriteInformation)
        {
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Summary:");
        WriteSummary(jwt);
    }

    private static void WriteHeaders(JwtSecurityToken jwt)
    {
        foreach (var header in jwt.Header)
        {
            Console.WriteLine("    " + header.Key + ": " + header.Value);
        }
    }

    private static void WritePayload(JwtSecurityToken jwt)
    {
        foreach (var claim in jwt.Payload)
        {
            Console.WriteLine("    " + claim.Key + ": " + claim.Value);
        }
    }

    private static void WriteSummary(JwtSecurityToken jwt)
    {
        // Get algorithm and other data from JWT token
        Console.WriteLine("    Algorithm: " + jwt.Header.Alg);
        if (!string.IsNullOrWhiteSpace(jwt.Issuer))
        {
            Console.WriteLine("    Issuer: " + jwt.Issuer);
        }

        if (jwt.IssuedAt != DateTime.MinValue)
        {
            Console.WriteLine("    Issued at: " + jwt.IssuedAt);
        }

        if (!string.IsNullOrWhiteSpace(jwt.Id))
        {
            Console.WriteLine("    Id: " + jwt.Id);
        }

        if (jwt.Audiences.Any())
        {
            Console.WriteLine("    Audience: " + string.Join(", ", jwt.Audiences));
        }

        if (!string.IsNullOrWhiteSpace(jwt.Subject))
        {
            Console.WriteLine("    Subject: " + jwt.Subject);
        }

        if (!string.IsNullOrWhiteSpace(jwt.Actor))
        {
            Console.WriteLine("    Actor: " + jwt.Actor);
        }

        if (jwt.ValidTo != DateTime.MinValue)
        {
            Console.WriteLine("    Expiration: " + jwt.ValidTo);
        }
    }
}