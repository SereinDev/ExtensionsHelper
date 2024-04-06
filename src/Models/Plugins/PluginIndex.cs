using System;

namespace ExtensionsHelper.Models.Plugins;

public class PluginIndex
{
    public string Owner { get; init; } = string.Empty;

    public string RepoName { get; init; } = string.Empty;

    public string Path { get; init; } = string.Empty;

    public string Branch { get; init; } = string.Empty;

    public Author[] Authors { get; init; } = Array.Empty<Author>();
}
