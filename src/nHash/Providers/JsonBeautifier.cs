using System.Text.Json;

namespace nHash.Providers;

public class JsonBeautifier
{
    private readonly JsonSerializerOptions _serializerOptions;

    public JsonBeautifier()
    {
        _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    public string Set(string text)
    {
        var jsonElement = JsonDocument.Parse(text).RootElement;
        var prettyJson = JsonSerializer.Serialize(jsonElement, _serializerOptions);
        return prettyJson;
    }
}