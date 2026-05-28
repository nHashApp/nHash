namespace nHash.Application.Encodes.Models;

public record JwtTokenResponse(string Header, string Payload, JwtTokenSummary? Summary);