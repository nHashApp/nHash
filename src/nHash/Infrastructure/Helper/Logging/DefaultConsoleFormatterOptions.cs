using Microsoft.Extensions.Logging.Console;

namespace nHash.Infrastructure.Helper.Logging;

public class DefaultConsoleFormatterOptions : ConsoleFormatterOptions
{
    public DefaultConsoleFormatterOptions()
    {
        TimestampFormat = "HH:mm:ss ";
        IncludeScopes = true;
    }
}