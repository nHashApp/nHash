using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using nHash.Providers;

namespace nHash.SubFeatures.Encodes;

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
        var command = new Command("jwt", "JWT token decode (Comply with GDPR rules)")
        {
            _noWriteInformation
        };
        command.AddArgument(_textArgument);
        command.SetHandler(DecodeJwtToken, _textArgument, _noWriteInformation);

        return command;
    }

    private static void DecodeJwtToken(string text, bool noWriteInformation)
    {
        var parts = text.Split('.');
        var header = parts[0];
        var payload = parts[1];
        //var signature = parts[2];


        var decodedHeader = HttpUtility.UrlDecode(Encoding.UTF8.GetString(Convert.FromBase64String(header)));
        payload = payload.PadRight(payload.Length + (payload.Length * 3) % 4, '=');
        var decodedPayload = HttpUtility.UrlDecode(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));

        //Console.WriteLine("JWT payload: " + decodedPayload);
        var jsonBeautifier = new JsonTools();
        var prettyHeader = jsonBeautifier.SetBeautiful(decodedHeader);
        var prettyPayload = jsonBeautifier.SetBeautiful(decodedPayload);

        Console.WriteLine();
        Console.WriteLine("Header: (ALGORITHM & TOKEN TYPE)");
        Console.WriteLine(prettyHeader);
        
        Console.WriteLine();
        Console.WriteLine("Payload: (DATA)");
        Console.WriteLine(prettyPayload);
        
        //WritePayload(jwt);

        if (noWriteInformation)
        {
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Summary:");
        WriteSummary(decodedHeader, decodedPayload);
        //Console.WriteLine("JWT algorithm: " + JsonObject.Parse(decodedHeader)["alg"]);
        //jwtObject.has
    }

    /*private static void WriteHeaders(JwtSecurityToken jwt)
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
    }*/

    private static void WriteSummary(string header, string payload)
    {
        var jwtObjectHeader = JsonNode.Parse(header);
        if (jwtObjectHeader is null)
        {
            return;
        }

        var algorithm = jwtObjectHeader["alg"];
        if (algorithm is not null)
        {
            Console.WriteLine("    Algorithm: " + algorithm);
        }

        var jwtObjectPayload = JsonNode.Parse(payload);
        if (jwtObjectPayload is null)
        {
            return;
        }

        var issuer = jwtObjectPayload["iss"];
        if (issuer is not  null)
        {
            Console.WriteLine("    Issuer: " + issuer);
        }

        var issuedAt = jwtObjectPayload["iat"];
        if (issuedAt is not  null)
        {
            var issueValue = Convert.ToInt64(issuedAt.ToString());
            Console.WriteLine("    Issued at: " + DateTimeOffset.FromUnixTimeSeconds(issueValue).DateTime);
        }

        var id = jwtObjectPayload["id"];
        if (id is not null)
        {
            Console.WriteLine("    Id: " + id);
        }

        var audience = jwtObjectPayload["aud"];
        if (audience is not null)
        {
            Console.WriteLine("    Audience: " + audience);
        }

        var subject = jwtObjectPayload["sub"];
        if (subject is not null)
        {
            Console.WriteLine("    Subject: " + subject);
        }

        var expirationAt = jwtObjectPayload["exp"];
        if (expirationAt is not null)
        {
            var expirationValue = Convert.ToInt64(expirationAt.ToString());
            Console.WriteLine("    Expiration: " + DateTimeOffset.FromUnixTimeSeconds(expirationValue).DateTime);
        }
    }
}