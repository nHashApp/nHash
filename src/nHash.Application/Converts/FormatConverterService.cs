using nHash.Application.Shared.Conversions;

namespace nHash.Application.Converts;

public class FormatConverterService : IFormatConverterService
{
    public string Convert(string input, string fromFormat, string toFormat)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var from = ParseFormat(fromFormat);
        var to = ParseFormat(toFormat);

        if (from == null || to == null)
        {
            return $"Error: Unsupported or invalid conversion formats. From: {fromFormat}, To: {toFormat}. Support: json, yaml, xml.";
        }

        try
        {
            return to.Value switch
            {
                ConversionType.Json => Conversion.ToJson(input, from.Value),
                ConversionType.Yaml => Conversion.ToYaml(input, from.Value),
                ConversionType.Xml => Conversion.ToXml(input, from.Value),
                _ => $"Conversion from '{fromFormat}' to '{toFormat}' is not supported."
            };
        }
        catch (Exception ex)
        {
            return $"Error during conversion: {ex.Message}";
        }
    }

    private static ConversionType? ParseFormat(string format)
    {
        return format.ToLowerInvariant().Trim() switch
        {
            "json" => ConversionType.Json,
            "yaml" => ConversionType.Yaml,
            "xml" => ConversionType.Xml,
            _ => null
        };
    }
}
