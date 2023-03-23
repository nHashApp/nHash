using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using nHash.Application.Abstraction;
using nHash.Application.Shared.Json;

namespace nHash.Application.Encodes;

public class JwtTokenService : IJwtTokenService
{
    private readonly IJsonTools _jsonTools;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IOutputProvider _outputProvider;

    public JwtTokenService(IJsonTools jsonTools, IDateTimeProvider timeProvider, IOutputProvider outputProvider)
    {
        _jsonTools = jsonTools;
        _timeProvider = timeProvider;
        _outputProvider = outputProvider;
    }

    public void DecodeJwtToken(string text, bool noWriteInformation)
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
        _outputProvider.AppendLine();
        _outputProvider.AppendLine("Header: (ALGORITHM & TOKEN TYPE)");
        _outputProvider.AppendLine(prettyHeader);

        _outputProvider.AppendLine();
        _outputProvider.AppendLine("Payload: (DATA)");
        _outputProvider.AppendLine(prettyPayload);

        if (noWriteInformation)
        {
            return;
        }

        _outputProvider.AppendLine();
        _outputProvider.AppendLine("Summary:");
        WriteSummary(decodedHeader, decodedPayload);
    }

    private void WriteSummary(string header, string payload)
    {
        WriteSummaryHeader(header);
        WriteSummaryPayload(payload);
    }

    private void WriteSummaryHeader(string header)
    {
        var jwtObjectHeader = JsonNode.Parse(header);

        var algorithm = jwtObjectHeader?["alg"];
        if (algorithm != null)
        {
            _outputProvider.AppendLine("    Algorithm: " + algorithm);
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
            _outputProvider.AppendLine("    Issuer: " + issuer);
        }

        var issuedAt = jwtObjectPayload["iat"];
        if (issuedAt is not null)
        {
            var issueValue = Convert.ToInt64(issuedAt.ToString());
            _outputProvider.AppendLine("    Issued at: " + _timeProvider.FromUnixTime(issueValue));
        }

        var id = jwtObjectPayload["id"];
        if (id is not null)
        {
            _outputProvider.AppendLine("    Id: " + id);
        }

        var audience = jwtObjectPayload["aud"];
        if (audience is not null)
        {
            _outputProvider.AppendLine("    Audience: " + audience);
        }

        var subject = jwtObjectPayload["sub"];
        if (subject is not null)
        {
            _outputProvider.AppendLine("    Subject: " + subject);
        }

        var expirationAt = jwtObjectPayload["exp"];
        if (expirationAt is not null)
        {
            var expirationValue = Convert.ToInt64(expirationAt.ToString());
            _outputProvider.AppendLine("    Expiration: " + _timeProvider.FromUnixTime(expirationValue));
        }
    }
}