using nHash.Console.CommandLines;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.Base;

public interface IFeature
{
    public BaseCommand Command { get; }
}