namespace nHash.Application.Ids;

public class NanoIdResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public List<string> NanoIds { get; set; } = new();
}

public interface INanoIdService
{
    NanoIdResult Generate(int length, string alphabet, int count);
}
