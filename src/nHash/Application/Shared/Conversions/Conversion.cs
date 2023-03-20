namespace nHash.Application.Shared.Conversions;

public static class Conversion
{
    public static string ToYaml(string value, ConversionType sourceType)
    {
        return new YamlConversion().From(value, sourceType);
    }
    
    public static string ToJson(string value, ConversionType sourceType)
    {
        return new JsonConversion().From(value, sourceType);
    }
}