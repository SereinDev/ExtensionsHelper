using System.Text.Encodings.Web;
using System.Text.Json;

namespace ExtensionsHelper.Utils;

public static class JsonSerializerOptionsFactory
{
    public static readonly JsonSerializerOptions Json =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
}
