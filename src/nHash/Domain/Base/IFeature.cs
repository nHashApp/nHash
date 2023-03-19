namespace nHash.Domain.Base;

public interface IFeature
{
    public Command Command { get; }
}