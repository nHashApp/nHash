namespace nHash.Application.Encodes.Models;

public class JwtTokenSummary
{
    public string? Algorithm { get; set; }
    public string? Issuer { get; set; }
    public DateTime? IssuedAt { get; set; }
    public string? Id { get; set; }
    public string? Audience { get; set; }
    public string? Subject { get; set; }
    public DateTime? Expiration { get; set; }
}