using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using ExtensionsHelper.Models.Plugins;
using ExtensionsHelper.Utils;

using Microsoft.Extensions.Logging;

namespace ExtensionsHelper.Services;

public class RawContentHttpClient : HttpClient
{
    private readonly RepoProvider _repoProvider;
    private readonly ILogger _logger;

    public RawContentHttpClient(RepoProvider repoProvider, ILogger logger)
    {
        BaseAddress = new Uri("https://raw.githubusercontent.com");
        _repoProvider = repoProvider;
        _logger = logger;
    }

    public async Task<PluginInfo> GetPluginInfo(
        string owner,
        string name,
        string? branch = default,
        string? path = default
    )
    {
        branch = string.IsNullOrEmpty(branch) ? (await _repoProvider.GetWithCache(owner, name)).DefaultBranch : branch;
        path = string.IsNullOrEmpty(path)
            ? "plugin-info.json"
            : $"{path.Trim('.', '/')}/plugin-info.json";

        var url = $"{owner}/{name}/{branch}/{path}";

        _logger.LogDebug("URL: {}", url);

        return await this.GetFromJsonAsync<PluginInfo>(
            url,
                options: new(JsonSerializerOptionsFactory.Json)
                {
                    Converters = { new JsonStringEnumConverter<Tags>() }
                }
            ) ?? throw new InvalidOperationException("plugin-info.json 为空");
    }
}
