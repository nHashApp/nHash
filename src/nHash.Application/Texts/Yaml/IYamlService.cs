using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Yaml;

public interface IYamlService
{
    void CalculateText(string text, ConversionType conversion);
}