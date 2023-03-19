using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace nHash.Infrastructure.Helper.Logging;

public static class LoggingExt
{
    private static readonly LogLevel _logLevel = LogLevel.Information; 
    public static IServiceCollection AddLoggingExt(this IServiceCollection services)
    {
        try
        {
            services.AddLogging(_ => _
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
                .SetMinimumLevel(_logLevel)
                .AddConsole(_ => _.FormatterName = DefaultConsoleFormatter.DefaultConsoleFormatterName)
                .AddConsoleFormatter<DefaultConsoleFormatter, DefaultConsoleFormatterOptions>()
            );

            return services;
        }
        catch (Exception e)
        {
            Console.Write("Internal error!");
            if (_logLevel <= LogLevel.Debug)
            {
                Console.WriteLine(JsonSerializer.Serialize(e));
            }

            Environment.Exit(ExitCodeConst.SettingLogError);
            return services;
        }
    }
}