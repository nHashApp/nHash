namespace nHash.Application.Ids;

public interface ICuidService
{
    string Generate(int length = 24);
}
