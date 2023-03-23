using nHash.Application.Shared.Conversions;

namespace nHash.Application.Texts.Yaml;

public interface IYamlService 
{
    Task CalculateText(string text, string fileName, ConversionType conversion);
}