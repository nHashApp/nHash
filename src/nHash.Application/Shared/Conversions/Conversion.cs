namespace nHash.Application.Shared.Conversions;

public static class Conversion
{
    public static string ToYaml(string value, ConversionType sourceType)
        => new YamlConversion().From(value, sourceType);
    
    public static string ToJson(string value, ConversionType sourceType)
        => new JsonConversion().From(value, sourceType);
    
    public static string ToXml(string value, ConversionType sourceType)
        => new XmlConversion().From(value, sourceType);
}