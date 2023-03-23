using nHash.Application.Shared.Conversions;
using nHash.Application.Texts.Json.Models;

namespace nHash.Application.Texts.Json;

public interface IJsonService 
{
    Task CalculateText(string text, JsonPrintType printType, string fileName,
        ConversionType conversion);
}