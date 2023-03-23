namespace nHash.Application.Uuids;

public interface IUUIDGenerator
{
    Guid GenerateUUIDv1();
    Guid GenerateUUIDv2();
    Guid GenerateUUIDv3(Guid namespaceId, string name);
    Guid GenerateUUIDv4();
    Guid GenerateUUIDv5(Guid namespaceId, string name);
}