namespace nHash.Application.Abstraction;

public interface IOutputProvider
{
    void Append(string text);
    void AppendLine(string text);
    Task WriteOutput();
}