using nHash.Application.Shared.Conversions;
using nHash.Application.Texts.Json.Models;

namespace nHash.Application.Texts.Json;

public interface IJsonService
{
    void CalculateText(string text, JsonPrintType printType, ConversionType conversion);
}