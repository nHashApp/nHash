using System.Text.Json;
using System.Xml.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace nHash.Application.Shared.Conversions;

public class JsonConversion : IConversion
{
    public string From(string value, ConversionType sourceType)
    {
        return sourceType switch
        {
            ConversionType.JSON => value,
            ConversionType.YAML => FromYaml(value),
            ConversionType.XML => FromXml(value),
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

        var json = JsonSerializer.Serialize(yamlObject, jsonOptions);
        return json;
    }

    private static string FromXml(string xml)
    {
        var doc = XDocument.Parse(xml);
        var result = new Dictionary<string, object>();
        if (doc?.Root is null)
        {
            return string.Empty;
        }
        ConvertNode(result, doc.Root);
        return JsonSerializer.Serialize(result);
    }

    private static void ConvertNode(Dictionary<string, object> obj, XElement element)
    {
        var nodeName = element.Name.LocalName;
        var attributes = element.Attributes().ToDictionary(a => a.Name.LocalName, a => (object)a.Value);

        if (element.HasElements)
        {
            var children = new List<Dictionary<string, object>>();
            foreach (var child in element.Elements())
            {
                var childObj = new Dictionary<string, object>();
                ConvertNode(childObj, child);
                children.Add(childObj);
            }

            obj.Add(nodeName, new { attributes, children });
        }
        else
        {
            obj.Add(nodeName, attributes.Count > 0 ? (object)attributes : element.Value);
        }
    }
}