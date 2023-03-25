using nHash.Application.Hashes.Models;
using nHash.Console.CommandLines.Encodes;
using nHash.Console.CommandLines.Encodes.SubCommands;
using nHash.Console.CommandLines.Hashes;
using nHash.Console.CommandLines.Hashes.SubCommands;
using nHash.Console.CommandLines.Passwords;
using nHash.Console.CommandLines.Texts;
using nHash.Console.CommandLines.Texts.SubCommands;
using nHash.Console.CommandLines.Uuids;

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
        
        services.AddSingleton<IOutputProvider, OutputProvider>();

        return services;
    }

    private static void RegisterEncodeServices(IServiceCollection services)
    {
        services.AddSingleton<IEncodeCommand, EncodeCommand>();
        services.AddSingleton<IBase64Command, Base64Command>();
        services.AddSingleton<IHtmlCommand, HtmlCommand>();
        services.AddSingleton<IJwtTokenCommand, JwtTokenCommand>();
        services.AddSingleton<IUrlCommand, UrlCommand>();
    }

    private static void RegisterHashServices(IServiceCollection services)
    {
        services.AddSingleton<IHashCommand, HashCommand>();
        services.AddSingleton<ICalcCommand, CalcCommand>();
        services.AddSingleton<IChecksumCommand, ChecksumCommand>();
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
    }

    private static void RegisterUuidServices(IServiceCollection services)
    {
        services.AddSingleton<IUuidCommand, UuidCommand>();

    }
}