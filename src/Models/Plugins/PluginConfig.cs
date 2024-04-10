using System;

namespace ExtensionsHelper.Models.Plugins;

public class PluginConfig
{
    public string[] NetAssemblies { get; init; } = Array.Empty<string>();
    public bool Debug { get; init; }
    public bool AllowGetType { get; init; }
    public bool AllowOperatorOverloading { get; init; } = true;
    public bool AllowSystemReflection { get; init; }
    public bool AllowWrite { get; init; } = true;
    public bool Strict { get; init; }
    public bool StringCompilationAllowed { get; init; } = true;
}
