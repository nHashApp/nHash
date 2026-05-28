namespace nHash.Application.Converts;

public interface IFormatConverterService
{
    string Convert(string input, string fromFormat, string toFormat);
}
