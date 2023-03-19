using nHash.Application.Encodes;
using nHash.Application.Hashes;
using nHash.Application.Helper.Json;
using nHash.Application.Passwords;
using nHash.Application.Texts;
using nHash.Application.Uuids;

namespace nHash.Application;

public static class ConfigureServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IUuidFeature, UuidFeature>();
        services.AddSingleton<IEncodeFeature, EncodeFeature>();
        services.AddSingleton<IHashFeature, HashFeature>();
        services.AddSingleton<ITextFeature, TextFeature>();
        services.AddSingleton<IPassGenerator, PassGenerator>();
        
        services.AddSingleton<IJsonTools, JsonTools>();
        services.AddSingleton<IUUIDGenerator, UUIDGenerator>();
        services.AddSingleton<IUrlFeature, UrlFeature>();
        services.AddSingleton<IHtmlFeature, HtmlFeature>();
        services.AddSingleton<IBase64Feature, Base64Feature>();
        services.AddSingleton<IJwtTokenFeature, JwtTokenFeature>();
    }
}