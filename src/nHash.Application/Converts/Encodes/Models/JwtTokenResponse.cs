namespace nHash.Application.Converts.Encodes.Models;

public record JwtTokenResponse(string Header, string Payload, JwtTokenSummary? Summary);