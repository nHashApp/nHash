using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace nHash.Application.Shared.Conversions;

public class XmlConversion : IConversion
{
    public string From(string value, ConversionType sourceType)
    {
        return sourceType switch
        {
            ConversionType.XML => value,
            ConversionType.JSON => FromJson(value),
            ConversionType.YAML => FromYaml(value),
            _ => value
        };
    }

    private static string FromJson(string json)
    {
        var xmlDoc = new XDocument();
        var root = new XElement("root");
        xmlDoc.Add(root);

        using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), new XmlDictionaryReaderQuotas()))
        {
            while (jsonReader.Read())
            {
                switch (jsonReader.NodeType)
                {
                    case XmlNodeType.Element:
                        var elementName = jsonReader.LocalName;
                        var element = new XElement(elementName);
                        root.Add(element);
                        if (!jsonReader.IsEmptyElement)
                        {
                            jsonReader.Read();
                            while (jsonReader.NodeType != XmlNodeType.EndElement)
                            {
                                element.Add(XNode.ReadFrom(jsonReader));
                            }
                        }
                        break;
                    case XmlNodeType.EndElement:
                        root = root.Parent;
                        break;
                    default:
                        break;
                }
            }
        }

        using (var memoryStream = new MemoryStream())
        {
            var xmlWriterSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = true
            };
            using (var xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
            {
                xmlDoc.WriteTo(xmlWriter);
                xmlWriter.Flush();
            }
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }

    private static string FromYaml(string yaml)
    {
        var json=Conversion.ToJson(yaml, ConversionType.YAML);
        return FromJson(json);
    }
    
    


}