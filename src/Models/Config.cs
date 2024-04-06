using Microsoft.Extensions.Logging;

namespace ExtensionsHelper.Models;

public class Config
{
    public bool InGitHubAction { get; init; }

    public LogLevel LogLevel { get; init; } = LogLevel.Information;

    public string Path { get; init; } = string.Empty;

    public string? Token { get; init; }
}
