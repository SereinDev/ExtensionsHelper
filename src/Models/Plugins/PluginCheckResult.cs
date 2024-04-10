namespace ExtensionsHelper.Models.Plugins;

public class PluginCheckResult
{
    public PluginCheckResult(string id, bool success, PluginIndex? pluginIndex = null, PluginInfo? pluginInfo = null, string? reason = null)
    {
        Id = id;
        Success = success;
        PluginIndex = pluginIndex;
        PluginInfo = pluginInfo;
        Reason = reason;
    }

    public string Id { get; }
    public bool Success { get; }
    public PluginIndex? PluginIndex { get; }
    public PluginInfo? PluginInfo { get; }
    public string? Reason { get; }
}
