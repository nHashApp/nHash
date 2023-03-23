using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Xml;

public interface IXmlService 
{
    Task CalculateText(string text, string fileName, ConversionType conversion);
}