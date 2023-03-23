using nHash.Application.Texts.Humanizers.Models;

namespace nHash.Application.Texts.Humanizers;

public interface IHumanizeService
{
    void CalculateText(string text, HumanizeType humanizeType);
}