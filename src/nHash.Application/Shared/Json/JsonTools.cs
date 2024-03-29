using System.Text.Json;

namespace nHash.Application.Shared.Json;

public class JsonTools : IJsonTools
{
    private readonly JsonSerializerOptions _beautifulSerializerOptions;
    private readonly JsonSerializerOptions _compactSerializerOptions;

    public JsonTools()
    {
        _beautifulSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        
        _compactSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = true
        };
    }

    public string SetBeautiful(string text)
    {
        var jsonElement = JsonDocument.Parse(text).RootElement;
        var prettyJson = JsonSerializer.Serialize(jsonElement, _beautifulSerializerOptions);
        return prettyJson;
    }    
    
    public string SetCompact(string text)
    {
        var jsonElement = JsonDocument.Parse(text).RootElement;
        var prettyJson = JsonSerializer.Serialize(jsonElement, _compactSerializerOptions);
        return prettyJson;
    }
    

}