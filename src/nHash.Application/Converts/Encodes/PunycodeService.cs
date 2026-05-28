using System.Globalization;

namespace nHash.Application.Encodes;

public class PunycodeService : IPunycodeService
{
    private static readonly IdnMapping IdnMapping = new();

    public string Calculate(string text, bool decode)
    {
        return decode ? Decode(text) : Encode(text);
    }

    private static string Encode(string text)
    {
        try
        {
            return IdnMapping.GetAscii(text);
        }
        catch (ArgumentException ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    private static string Decode(string text)
    {
        try
        {
            return IdnMapping.GetUnicode(text);
        }
        catch (ArgumentException ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}
