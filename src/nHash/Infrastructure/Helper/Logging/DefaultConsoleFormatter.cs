using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace nHash.Infrastructure.Helper.Logging;

internal sealed class DefaultConsoleFormatter : ConsoleFormatter, IDisposable
{
    public const string DefaultConsoleFormatterName = "default";
    private readonly IDisposable? _optionsReloadToken;
    private DefaultConsoleFormatterOptions _formatterOptions;

    private const string DefaultForegroundColor = "\x1B[39m\x1B[22m";

    public DefaultConsoleFormatter(IOptionsMonitor<DefaultConsoleFormatterOptions> options) : base(
        DefaultConsoleFormatterName)
    {
        _optionsReloadToken = options.OnChange(ReloadLoggerOptions);
        _formatterOptions = options.CurrentValue;
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        var message = logEntry.Formatter(logEntry.State, logEntry.Exception);

        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }


        var time = DateTime.Now.ToString(_formatterOptions.TimestampFormat);

        var logText = new StringBuilder();
        logText.Append($"{DefaultForegroundColor}{time}");
        logText.Append($"{GetLogLevelColor(logEntry.LogLevel)}{GetLevelName(logEntry.LogLevel)} ");
        logText.Append($"{GetForegroundColorEscapeCode(ConsoleColor.Gray)}[{logEntry.Category}] ");
        logText.Append($"{DefaultForegroundColor}{message}");

        textWriter.WriteLine(logText.ToString());
    }

    public void Dispose() =>
        _optionsReloadToken?.Dispose();


    private void ReloadLoggerOptions(DefaultConsoleFormatterOptions options) =>
        _formatterOptions = options;

    private static string GetLevelName(LogLevel level) =>
        level switch
        {
            LogLevel.Trace => "TRACE",
            LogLevel.Debug => "DEBUG",
            LogLevel.Information => "INFO ",
            LogLevel.Warning => "WARN ",
            LogLevel.Error => "ERROR",
            LogLevel.Critical => "FATAL",
            _ => "NONE "
        };

    private static string GetForegroundColorEscapeCode(ConsoleColor color) =>
        color switch
        {
            ConsoleColor.Black => "\x1B[30m",
            ConsoleColor.DarkRed => "\x1B[31m",
            ConsoleColor.DarkGreen => "\x1B[32m",
            ConsoleColor.DarkYellow => "\x1B[33m",
            ConsoleColor.DarkBlue => "\x1B[34m",
            ConsoleColor.DarkMagenta => "\x1B[35m",
            ConsoleColor.DarkCyan => "\x1B[36m",
            ConsoleColor.Gray => "\x1B[37m",
            ConsoleColor.Red => "\x1B[1m\x1B[31m",
            ConsoleColor.Green => "\x1B[1m\x1B[32m",
            ConsoleColor.Yellow => "\x1B[1m\x1B[33m",
            ConsoleColor.Blue => "\x1B[1m\x1B[34m",
            ConsoleColor.Magenta => "\x1B[1m\x1B[35m",
            ConsoleColor.Cyan => "\x1B[1m\x1B[36m",
            ConsoleColor.White => "\x1B[1m\x1B[37m",

            _ => DefaultForegroundColor
        };

    private static string GetLogLevelColor(LogLevel level) =>
        level switch
        {
            LogLevel.Trace => GetForegroundColorEscapeCode(ConsoleColor.Green),
            LogLevel.Debug => GetForegroundColorEscapeCode(ConsoleColor.Cyan),
            LogLevel.Information => GetForegroundColorEscapeCode(ConsoleColor.DarkCyan),
            LogLevel.Warning => GetForegroundColorEscapeCode(ConsoleColor.DarkYellow),
            LogLevel.Error => GetForegroundColorEscapeCode(ConsoleColor.DarkRed),
            LogLevel.Critical => GetForegroundColorEscapeCode(ConsoleColor.Red),
            _ => DefaultForegroundColor
        };
}