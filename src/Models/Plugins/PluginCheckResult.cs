namespace ExtensionsHelper.Models.Plugins;

public class PluginCheckResult
{
    public bool Success { get; init; }

    public string Id { get; set; } = string.Empty;

    public string? Reason { get; set; }
}
