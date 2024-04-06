using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using ExtensionsHelper.Models.Plugins;
using ExtensionsHelper.Utils;

namespace ExtensionsHelper.Services;

public class RawContentHttpClient : HttpClient
{
    private readonly RepoProvider _repoProvider;

    public RawContentHttpClient(RepoProvider repoProvider)
    {
        BaseAddress = new Uri("https://raw.githubusercontent.com/");
        _repoProvider = repoProvider;
    }

    public async Task<PluginInfo> GetPluginInfo(
        string owner,
        string name,
        string? branch = default,
        string? path = default
    )
    {
        branch ??= (await _repoProvider.GetWithCache(owner, name)).DefaultBranch;
        path = string.IsNullOrEmpty(path)
            ? "plugin-info.json"
            : $"{path.Trim('.', '/')}/plugin-info.json";

        return await this.GetFromJsonAsync<PluginInfo>(
                $"{owner}/{name}/{branch}/{path}",
                JsonSerializerOptionsFactory.Json
            ) ?? throw new NullReferenceException("plugin-info.json 为空");
    }
}
