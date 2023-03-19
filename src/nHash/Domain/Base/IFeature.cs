namespace nHash.Domain.Base;

public interface IFeature
{
    Command Command { get; }
}