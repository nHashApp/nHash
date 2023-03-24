using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Xml;

public interface IXmlService
{
    string CalculateText(string text, ConversionType conversion);
}