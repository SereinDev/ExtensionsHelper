using System;
using System.Text.Json.Serialization;

namespace ExtensionsHelper.Models.Plugins;

public class DependencyInfo
{
    [JsonRequired]
    public string Id { get; init; } = string.Empty;

    [JsonRequired]
    public Version Version { get; init; } = new();
}
