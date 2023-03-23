namespace nHash.Application.Shared.Conversions;

public interface IConversion
{
    string From(string value, ConversionType sourceType);
}