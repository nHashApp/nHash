using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Xml;

public interface IXmlService
{
    void CalculateText(string text, ConversionType conversion);
}