using System.Text.Json.Serialization;

namespace ExtensionsHelper.Models;

public class Author
{
    [JsonRequired]
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}
