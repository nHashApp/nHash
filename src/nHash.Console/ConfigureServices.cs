using nHash.Console.CommandLines.Encodes;
using nHash.Console.CommandLines.Encodes.SubCommands;
using nHash.Console.CommandLines.Hashes;
using nHash.Console.CommandLines.Hashes.SubCommands;
using nHash.Console.CommandLines.Passwords;
using nHash.Console.CommandLines.Texts;
using nHash.Console.CommandLines.Texts.SubCommands;
using nHash.Console.CommandLines.Uuids;
using nHash.Console.CommandLines.Ids;
using nHash.Console.CommandLines.Cryptos;
using nHash.Console.CommandLines.Converts;
using nHash.Console.CommandLines.Arts;
using nHash.Console.CommandLines.Network;
using nHash.Console.CommandLines.Date;
using nHash.Console.CommandLines.File;
using nHash.Console.CommandLines.Dev;

namespace nHash.Console;

public static class ConfigureServices
{
    public static IServiceCollection RegisterUiServices(this IServiceCollection services)
    {
        RegisterEncodeServices(services);
        RegisterHashServices(services);
        RegisterPasswordServices(services);
        RegisterTextServices(services);
        RegisterUuidServices(services);
        RegisterConvertServices(services);
        RegisterArtServices(services);
        RegisterNetworkServices(services);
        RegisterDateServices(services);
        RegisterFileServices(services);
        RegisterDevServices(services);
        
        services.AddSingleton<IOutputProvider, OutputProvider>();

        return services;
    }

    private static void RegisterEncodeServices(IServiceCollection services)
    {
        services.AddSingleton<IEncodeCommand, EncodeCommand>();
        services.AddSingleton<IBase64Command, Base64Command>();
        services.AddSingleton<IBase58Command, Base58Command>();
        services.AddSingleton<IHtmlCommand, HtmlCommand>();
        services.AddSingleton<IJwtTokenCommand, JwtTokenCommand>();
        services.AddSingleton<IUrlCommand, UrlCommand>();
        services.AddSingleton<IBase32Command, Base32Command>();
        services.AddSingleton<IHexCommand, HexCommand>();
        services.AddSingleton<IBase62Command, Base62Command>();
        services.AddSingleton<IBase85Command, Base85Command>();
        services.AddSingleton<IBase36Command, Base36Command>();
    }

    private static void RegisterHashServices(IServiceCollection services)
    {
        services.AddSingleton<IHashCommand, HashCommand>();
        services.AddSingleton<ICalcCommand, CalcCommand>();
        services.AddSingleton<IChecksumCommand, ChecksumCommand>();
        services.AddSingleton<ICryptoCommand, CryptoCommand>();
        services.AddSingleton<IHmacCommand, HmacCommand>();
        services.AddSingleton<ICipherCommand, CipherCommand>();
    }

    private static void RegisterPasswordServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordCommand, PasswordCommand>();

    }

    private static void RegisterTextServices(IServiceCollection services)
    {
        services.AddSingleton<ITextCommand, TextCommand>();
        services.AddSingleton<IHumanizeCommand, HumanizeCommand>();
        services.AddSingleton<IJsonCommand, JsonCommand>();
        services.AddSingleton<IYamlCommand, YamlCommand>();
        services.AddSingleton<IXmlCommand, XmlCommand>();
        services.AddSingleton<ICaseCommand, CaseCommand>();
        services.AddSingleton<IDiffCommand, DiffCommand>();
        services.AddSingleton<IStatsCommand, StatsCommand>();
        services.AddSingleton<ILoremCommand, LoremCommand>();
    }

    private static void RegisterUuidServices(IServiceCollection services)
    {
        services.AddSingleton<IUuidCommand, UuidCommand>();
        services.AddSingleton<IIdCommand, IdCommand>();
        services.AddSingleton<ISnowflakeCommand, SnowflakeCommand>();
        services.AddSingleton<ICuidCommand, CuidCommand>();
    }

    private static void RegisterConvertServices(IServiceCollection services)
    {
        services.AddSingleton<IConvertCommand, ConvertCommand>();
        services.AddSingleton<IFormatCommand, FormatCommand>();
        services.AddSingleton<IBaseNCommand, BaseNCommand>();
    }

    private static void RegisterArtServices(IServiceCollection services)
    {
        services.AddSingleton<IArtCommand, ArtCommand>();
        services.AddSingleton<IAsciiCommand, AsciiCommand>();
    }

    private static void RegisterNetworkServices(IServiceCollection services)
    {
        services.AddSingleton<INetworkCommand, NetworkCommand>();
    }

    private static void RegisterDateServices(IServiceCollection services)
    {
        services.AddSingleton<IDateCommand, DateCommand>();
    }

    private static void RegisterFileServices(IServiceCollection services)
    {
        services.AddSingleton<IFileCommand, FileCommand>();
    }

    private static void RegisterDevServices(IServiceCollection services)
    {
        services.AddSingleton<IDevCommand, DevCommand>();
    }
}