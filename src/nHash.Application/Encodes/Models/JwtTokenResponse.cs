namespace nHash.Application.Encodes.Models;

public record JwtTokenResponse(string Header, string Payload, string? Summary);