using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using nHash.Application.Abstraction;
using nHash.Application.Shared.Json;

namespace nHash.Application.Encodes;

public class JwtTokenFeature : IJwtTokenFeature
{
    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _noWriteInformation;
    private readonly Argument<string> _textArgument;

    private readonly IJsonTools _jsonTools;
    private readonly IDateTimeProvider _timeProvider;

    public JwtTokenFeature(IJsonTools jsonTools, IDateTimeProvider timeProvider)
    {
        _jsonTools = jsonTools;
        _timeProvider = timeProvider;

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
        var parts = text.Split('.');
        var header = parts[0];
        var payload = parts[1];
        //var signature = parts[2];

        var decodedHeader = HttpUtility.UrlDecode(Encoding.UTF8.GetString(Convert.FromBase64String(header)));
        payload = payload.PadRight(payload.Length + (payload.Length * 3) % 4, '=');
        var decodedPayload = HttpUtility.UrlDecode(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));

        var prettyHeader = _jsonTools.SetBeautiful(decodedHeader);
        var prettyPayload = _jsonTools.SetBeautiful(decodedPayload);

        WriteJwtInfo(noWriteInformation, prettyHeader, prettyPayload, decodedHeader, decodedPayload);
    }

    private void WriteJwtInfo(bool noWriteInformation, string prettyHeader, string prettyPayload, string decodedHeader,
        string decodedPayload)
    {
        Console.WriteLine();
        Console.WriteLine("Header: (ALGORITHM & TOKEN TYPE)");
        Console.WriteLine(prettyHeader);

        Console.WriteLine();
        Console.WriteLine("Payload: (DATA)");
        Console.WriteLine(prettyPayload);

        if (noWriteInformation)
        {
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Summary:");
        WriteSummary(decodedHeader, decodedPayload);
    }

    private void WriteSummary(string header, string payload)
    {
        WriteSummaryHeader(header);
        WriteSummaryPayload(payload);
    }

    private static void WriteSummaryHeader(string header)
    {
        var jwtObjectHeader = JsonNode.Parse(header);

        var algorithm = jwtObjectHeader?["alg"];
        if (algorithm != null)
        {
            Console.WriteLine("    Algorithm: " + algorithm);
        }
    }

    private void WriteSummaryPayload(string payload)
    {
        var jwtObjectPayload = JsonNode.Parse(payload);
        if (jwtObjectPayload is null)
        {
            return;
        }

        var issuer = jwtObjectPayload["iss"];
        if (issuer is not null)
        {
            Console.WriteLine("    Issuer: " + issuer);
        }

        var issuedAt = jwtObjectPayload["iat"];
        if (issuedAt is not null)
        {
            var issueValue = Convert.ToInt64(issuedAt.ToString());
            Console.WriteLine("    Issued at: " + _timeProvider.FromUnixTime(issueValue));
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
            Console.WriteLine("    Expiration: " + _timeProvider.FromUnixTime(expirationValue));
        }
    }
}