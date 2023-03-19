namespace nHash.Application.Shared.Json;

public interface IJsonTools
{
    string SetBeautiful(string text);
    string SetCompact(string text);
    string FromYaml(string yaml);
}