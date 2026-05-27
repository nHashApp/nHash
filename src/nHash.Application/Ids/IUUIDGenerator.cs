namespace nHash.Application.Uuids;

public interface IUuidGenerator
{
    Guid GenerateUuiDv1();
    Guid GenerateUuiDv2();
    Guid GenerateUuiDv3(Guid namespaceId, string name);
    Guid GenerateUuiDv4();
    Guid GenerateUuiDv5(Guid namespaceId, string name);
    Guid GenerateUuiDv7();
    Guid GenerateUuiDv8(ReadOnlySpan<byte> customData);
    string GenerateUlid();
    string GenerateNanoId(int size = 21);
}