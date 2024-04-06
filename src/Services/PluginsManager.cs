using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using ExtensionsHelper.Models;
using ExtensionsHelper.Models.Plugins;
using ExtensionsHelper.Utils;

using Microsoft.Extensions.Logging;

namespace ExtensionsHelper.Services;

public class PluginsManager
{
    private readonly Config _config;
    private readonly ILogger _logger;
    private readonly UserProvider _userChecker;
    private readonly RawContentHttpClient _rawContentHttpClient;
    private readonly string _path;

    public PluginsManager(
        Config config,
        ILogger logger,
        UserProvider userChecker,
        RawContentHttpClient rawContentHttpClient
    )
    {
        _config = config;
        _logger = logger;
        _userChecker = userChecker;
        _rawContentHttpClient = rawContentHttpClient;
        _path = Path.Combine(_config.Path, "src", "plugins");

        if (!Directory.Exists(_path))
            throw new NotSupportedException($"插件路径\"{_path}\"不存在");
    }

    public async Task Check()
    {
        var list = new List<PluginCheckResult>();

        foreach (var path in Directory.GetDirectories(_path))
        {
            var id = Path.GetFileName(path);
            try { }
            catch (Exception e)
            {
                _logger.LogError(e, "[{}] 插件检查失败", id);
            }
        }
    }

    // public async void CheckPlugin(string path)
    // {
    //     var jsonPath = Path.Combine(path, "plugin-index.json");
    //     if (!File.Exists(jsonPath))
    //         throw new InvalidOperationException("文件夹中不存在 plugin-index.json");
    // }

    public void CheckIndex(string content)
    {
        var index =
            JsonSerializer.Deserialize<PluginIndex>(content, JsonSerializerOptionsFactory.Json)
            ?? throw new NullReferenceException("Json文件为空");

        Ensure.NotEmpty(index.Authors, nameof(index.Authors));
        Ensure.True(
            index.Authors.All(
                (author) => _userChecker.Exists(author.Name).GetAwaiter().GetResult()
            ),
            "存在不正确的用户"
        );
    }

    public async Task CheckInfo(string id, PluginIndex index)
    {
        var info = await _rawContentHttpClient.GetPluginInfo(
            index.Owner,
            index.RepoName,
            index.Branch,
            index.Path
        );

        Ensure.True(id == info.Id, "插件Id不匹配");
        Ensure.NotNullOrEmpty(info.Name, nameof(info.Name));
    }
}
