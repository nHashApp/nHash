using Microsoft.Extensions.DependencyInjection;
using nHash.Application.Converts;
using nHash.Application.Encodes;
using nHash.Application.Hashes;
using nHash.Application.Passwords;
using nHash.Application.Shared.Json;
using nHash.Application.Shared.Yaml;
using nHash.Application.Texts.Humanizers;
using nHash.Application.Texts.Json;
using nHash.Application.Texts.Xml;
using nHash.Application.Texts.Yaml;
using nHash.Application.Texts;
using nHash.Application.Uuids;
using nHash.Application.Ids;
using nHash.Application.Cryptos;
using nHash.Application.Cryptos.Hashes;
using nHash.Application.Network;
using nHash.Application.Date;
using nHash.Application.File;
using nHash.Application.Dev;
using nHash.Application.Sys;
using nHash.Application.Maths;

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
        RegisterConvertServices(services);
        RegisterNetworkServices(services);
        RegisterDateServices(services);
        RegisterFileServices(services);
        RegisterDevServices(services);
        RegisterSysServices(services);
        RegisterMathServices(services);

        return services;
    }

    private static void RegisterEncodeServices(IServiceCollection services)
    {
        services.AddSingleton<IBase64Service, Base64Service>();
        services.AddSingleton<IBase58Service, Base58Service>();
        services.AddSingleton<IHtmlService, HtmlService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IUrlService, UrlService>();
        services.AddSingleton<IBase32Service, Base32Service>();
        services.AddSingleton<IHexService, HexService>();
        services.AddSingleton<IBase62Service, Base62Service>();
        services.AddSingleton<IBase85Service, Base85Service>();
        services.AddSingleton<IBase36Service, Base36Service>();
        services.AddSingleton<IBase45Service, Base45Service>();
        services.AddSingleton<IBase91Service, Base91Service>();
        services.AddSingleton<IPunycodeService, PunycodeService>();
        services.AddSingleton<IRot13Service, Rot13Service>();
        services.AddSingleton<IMorseService, MorseService>();
        services.AddSingleton<IBinaryTextService, BinaryTextService>();
    }

    private static void RegisterHashServices(IServiceCollection services)
    {
        services.AddSingleton<IHashCalcService, HashCalcService>();
        services.AddSingleton<IChecksumService, ChecksumService>();
        services.AddSingleton<IHmacService, HmacService>();
        services.AddSingleton<ICipherService, CipherService>();
        services.AddSingleton<ISignatureService, SignatureService>();
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
        services.AddSingleton<ICaseConverterService, CaseConverterService>();
        services.AddSingleton<ITextDiffService, TextDiffService>();
        services.AddSingleton<ITextStatisticsService, TextStatisticsService>();
        services.AddSingleton<ILoremIpsumService, LoremIpsumService>();
        services.AddSingleton<ITextToolsService, TextToolsService>();
        
        services.AddSingleton<IJsonTools, JsonTools>();
        services.AddSingleton<IYamlTools, YamlTools>();
    }

    private static void RegisterUuidServices(IServiceCollection services)
    {
        services.AddSingleton<IUuidService, UuidService>();
        services.AddSingleton<IUuidGenerator, UuidGenerator>();
        services.AddSingleton<ISnowflakeService, SnowflakeService>();
        services.AddSingleton<ICuidService, CuidService>();
        services.AddSingleton<ITotpService, TotpService>();
        services.AddSingleton<IUuidInspectService, UuidInspectService>();
    }

    private static void RegisterConvertServices(IServiceCollection services)
    {
        services.AddSingleton<IFormatConverterService, FormatConverterService>();
        services.AddSingleton<IBaseNService, BaseNService>();
    }

    private static void RegisterNetworkServices(IServiceCollection services)
    {
        services.AddSingleton<INetworkService, NetworkService>();
    }

    private static void RegisterDateServices(IServiceCollection services)
    {
        services.AddSingleton<IDateService, DateService>();
    }

    private static void RegisterFileServices(IServiceCollection services)
    {
        services.AddSingleton<IFileService, FileService>();
    }

    private static void RegisterDevServices(IServiceCollection services)
    {
        services.AddSingleton<IDevService, DevService>();
    }

    private static void RegisterSysServices(IServiceCollection services)
    {
        services.AddSingleton<ISysService, SysService>();
    }

    private static void RegisterMathServices(IServiceCollection services)
    {
        services.AddSingleton<IMathService, MathService>();
    }
}