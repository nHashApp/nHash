using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace nHash.Application.Shared.Yaml;

public class YamlTools : IYamlTools
{
    public string FromJson(string json)
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
