using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Yaml;

public interface IYamlService
{
    string CalculateText(string text, ConversionType conversion);
}