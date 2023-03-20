using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace nHash.Application.Shared.Conversions;

public class YamlConversion : IConversion
{
    public string From(string value, ConversionType sourceType)
    {
        return sourceType switch
        {
            ConversionType.Yaml => value,
            ConversionType.Json => FromJson(value),
            _ => value
        };
    }
    
    private static string FromJson(string json)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var obj = deserializer.Deserialize<object>(json);

        var serializer = new SerializerBuilder()
            .DisableAliases()
            .Build();

        var yaml= serializer.Serialize(obj);
        return yaml;
    }
}