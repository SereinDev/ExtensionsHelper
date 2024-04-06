using System;
using System.Text;

using ExtensionsHelper.Models;

using Microsoft.Extensions.Logging;

namespace ExtensionsHelper.Utils;

public class CliLogger : ILogger
{
    private readonly LogLevel _logLevel;
    private readonly bool _inGitHubAction;

    public CliLogger(Config config)
    {
        _logLevel = config.LogLevel;
        _inGitHubAction = config.InGitHubAction;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel > _logLevel;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        var text = formatter(state, exception);
    }
}
