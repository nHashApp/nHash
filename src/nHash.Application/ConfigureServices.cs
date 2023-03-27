using Microsoft.Extensions.DependencyInjection;
using nHash.Application.Encodes;
using nHash.Application.Hashes;
using nHash.Application.Passwords;
using nHash.Application.Shared.Json;
using nHash.Application.Shared.Yaml;
using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Json;
using nHash.Application.Texts.Xml;
using nHash.Application.Texts.Yaml;
using nHash.Application.Uuids;

namespace nHash.Application;

public static class ConfigureServices
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        RegisterEncodeServices(services);
        RegisterHashServices(services);
        RegisterPasswordServices(services);
        RegisterTextServices(services);
        RegisterUuidServices(services);

        return services;
    }

    private static void RegisterEncodeServices(IServiceCollection services)
    {
        services.AddSingleton<IBase64Service, Base64Service>();
        services.AddSingleton<IBase58Service, Base58Service>();
        services.AddSingleton<IHtmlService, HtmlService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IUrlService, UrlService>();
    }

    private static void RegisterHashServices(IServiceCollection services)
    {
        services.AddSingleton<IHashCalcService, HashCalcService>();
        services.AddSingleton<IChecksumService, ChecksumService>();
    }

    private static void RegisterPasswordServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordService, PasswordService>();
    }

    private static void RegisterTextServices(IServiceCollection services)
    {
        services.AddSingleton<IHumanizeService, HumanizeService>();
        services.AddSingleton<IJsonService, JsonService>();
        services.AddSingleton<IYamlService, YamlService>();
        services.AddSingleton<IXmlService, XmlService>();
        
        services.AddSingleton<IJsonTools, JsonTools>();
        services.AddSingleton<IYamlTools, YamlTools>();
    }

    private static void RegisterUuidServices(IServiceCollection services)
    {
        services.AddSingleton<IUuidService, UuidService>();
        services.AddSingleton<IUUIDGenerator, UUIDGenerator>();
    }
}