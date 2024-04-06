using System;
using System.Text.Json.Serialization;

namespace ExtensionsHelper.Models.Datas;

public class DataInfo
{
    [JsonRequired]
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    [JsonRequired]
    public Author[] Authors { get; init; } = Array.Empty<Author>();

    [JsonRequired]
    public Version[] TargetingSerein { get; init; } = Array.Empty<Version>();
}
