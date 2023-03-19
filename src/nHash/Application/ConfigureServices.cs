using nHash.Application.Encodes;
using nHash.Application.Hashes;
using nHash.Application.Passwords;
using nHash.Application.Shared.Json;
using nHash.Application.Texts;
using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Json;
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
        services.AddSingleton<IPasswordFeature, PasswordFeature>();
        
        services.AddSingleton<IUUIDGenerator, UUIDGenerator>();
        services.AddSingleton<IUrlFeature, UrlFeature>();
        services.AddSingleton<IHtmlFeature, HtmlFeature>();
        services.AddSingleton<IBase64Feature, Base64Feature>();
        services.AddSingleton<IJwtTokenFeature, JwtTokenFeature>();
        services.AddSingleton<IHumanizeFeature, HumanizeFeature>();
        services.AddSingleton<IJsonFeature, JsonFeature>();
        
        services.AddSingleton<IJsonTools, JsonTools>();
        services.AddSingleton<IPasswordTools, PasswordTools>();
    }
}