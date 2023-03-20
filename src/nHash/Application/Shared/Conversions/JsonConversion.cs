using System.Text.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace nHash.Application.Shared.Conversions;

public class JsonConversion : IConversion
{
   
    public string From(string value, ConversionType sourceType)
    {
        return sourceType switch
        {
            ConversionType.Json => value,
            ConversionType.Yaml => FromYaml(value),
            _ => value
        };
    }
    
    private static string FromYaml(string yaml)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yamlObject = deserializer.Deserialize<object>(yaml);

        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json= JsonSerializer.Serialize(yamlObject, jsonOptions);
        Console.WriteLine(json);
        return json;
    }
}