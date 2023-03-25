using nHash.Application.Texts.Humanizers.Models;

namespace nHash.Application.Texts.Humanizers;

public interface IHumanizeService
{
    string CalculateText(string text, HumanizeType humanizeType);
}