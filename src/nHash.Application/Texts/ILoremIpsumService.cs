namespace nHash.Application.Texts;

public interface ILoremIpsumService
{
    string Generate(int count, string type);
}
