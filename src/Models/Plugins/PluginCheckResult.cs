namespace ExtensionsHelper.Models.Plugins;

public record PluginCheckResult(string Id, bool Success, string? Reason = null)
{

}
