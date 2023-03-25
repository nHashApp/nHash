using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using nHash.Application.Abstraction;
using nHash.Application.Encodes.Models;
using nHash.Application.Shared.Json;

namespace nHash.Application.Encodes;

public class JwtTokenService : IJwtTokenService
{
    private readonly IJsonTools _jsonTools;
    private readonly IDateTimeProvider _timeProvider;

    public JwtTokenService(IJsonTools jsonTools, IDateTimeProvider timeProvider)
    {
        _jsonTools = jsonTools;
        _timeProvider = timeProvider;
    }

    public JwtTokenResponse DecodeJwtToken(string text, bool noWriteInformation)
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

        JwtTokenSummary? summary = null;
        if (!noWriteInformation)
        {
            summary = WriteSummary(decodedHeader, decodedPayload);
        }

        return new JwtTokenResponse(prettyHeader, prettyPayload, summary ?? new JwtTokenSummary());
    }

    private JwtTokenSummary WriteSummary(string header, string payload)
    {
        var headerResult = WriteSummaryHeader(header);
        var payloadResult = WriteSummaryPayload(payload);

        payloadResult.Algorithm = headerResult.Algorithm;
        return payloadResult;
    }

    private static JwtTokenSummary WriteSummaryHeader(string header)
    {
        var result = new JwtTokenSummary();
        var jwtObjectHeader = JsonNode.Parse(header);

        var algorithm = jwtObjectHeader?["alg"];
        if (algorithm != null)
        {
            result.Algorithm = algorithm.ToString();
        }

        return result;
    }

    private JwtTokenSummary WriteSummaryPayload(string payload)
    {
        var result = new JwtTokenSummary();

        var jwtObjectPayload = JsonNode.Parse(payload);
        if (jwtObjectPayload is null)
        {
            return result;
        }

        var issuer = jwtObjectPayload["iss"];
        if (issuer is not null)
        {
            result.Issuer = issuer.ToString();
        }

        var issuedAt = jwtObjectPayload["iat"];
        if (issuedAt is not null)
        {
            var issueValue = Convert.ToInt64(issuedAt.ToString());
            result.IssuedAt = _timeProvider.FromUnixTime(issueValue);
        }

        var id = jwtObjectPayload["id"];
        if (id is not null)
        {
            result.Id = id.ToString();
        }

        var audience = jwtObjectPayload["aud"];
        if (audience is not null)
        {
            result.Audience = audience.ToString();
        }

        var subject = jwtObjectPayload["sub"];
        if (subject is not null)
        {
            result.Subject = subject.ToString();
        }

        var expirationAt = jwtObjectPayload["exp"];
        if (expirationAt is not null)
        {
            var expirationValue = Convert.ToInt64(expirationAt.ToString());
            result.Expiration = _timeProvider.FromUnixTime(expirationValue);
        }

        return result;
    }
}