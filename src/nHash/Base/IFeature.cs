using System.CommandLine;

namespace nHash.Base;

public interface IFeature
{
    Command Command { get; }
}