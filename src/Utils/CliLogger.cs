using System;
using System.Text;

using ExtensionsHelper.Models;

using Microsoft.Extensions.Logging;

using Spectre.Console;

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
        return logLevel >= _logLevel;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {

        if (!IsEnabled(logLevel))
            return;

        var text = formatter(state, exception);

        switch (logLevel)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
                if (_inGitHubAction)
                    AnsiConsole.MarkupLineInterpolated($"::debug::[mediumpurple4]Debug[/] {text}");
                else
                    AnsiConsole.MarkupLineInterpolated($"[mediumpurple4]Debug[/] {text}");
                break;

            case LogLevel.Information:
                AnsiConsole.MarkupLineInterpolated($"Info  {text}");
                break;

            case LogLevel.Warning:
                if (_inGitHubAction)
                    AnsiConsole.MarkupLineInterpolated($"::warning::[yellow bold]Warn  {text}[/]");
                else
                    AnsiConsole.MarkupLineInterpolated($"[yellow bold]]Warn  {text}[/]");
                break;

            case LogLevel.Error:
            case LogLevel.Critical:
                if (_inGitHubAction)
                    AnsiConsole.MarkupLineInterpolated($"::error::[red bold]Error {text}[/]");
                else
                    AnsiConsole.MarkupLineInterpolated($"[red bold]Error {text}[/]");
                break;

            case LogLevel.None:
            default:
                break;
        }
    }
}
