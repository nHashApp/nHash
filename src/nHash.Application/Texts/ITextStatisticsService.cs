using nHash.Application.Texts.Models;

namespace nHash.Application.Texts;

public interface ITextStatisticsService
{
    TextStatisticsResult Calculate(string text);
}
