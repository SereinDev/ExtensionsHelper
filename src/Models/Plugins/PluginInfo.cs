using System;
using System.Text.Json.Serialization;

namespace ExtensionsHelper.Models.Plugins;

public class PluginInfo
{
    [JsonRequired]
    public string Name { get; init; } = string.Empty;

    [JsonRequired]
    public string Id { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    [JsonRequired]
    public bool IsModule { get; init; }

    public DependencyInfo[] Dependencies { get; init; } = Array.Empty<DependencyInfo>();

    public Tags[] Tags { get; init; } = Array.Empty<Tags>();

    [JsonRequired]
    public Version[] TargetingSerein { get; init; } = Array.Empty<Version>();

    public PluginConfig Config { get; init; } = new();
}
