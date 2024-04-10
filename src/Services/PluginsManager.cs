using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using ExtensionsHelper.Models;
using ExtensionsHelper.Models.Exceptions;
using ExtensionsHelper.Models.Plugins;
using ExtensionsHelper.Utils;

using Microsoft.Extensions.Logging;

using Spectre.Console;

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

    public async Task<IReadOnlyList<PluginCheckResult>> Check()
    {
        var list = new List<PluginCheckResult>();

        foreach (var path in Directory.GetDirectories(_path))
        {
            var id = Path.GetFileName(path);
            _logger.LogDebug("[{}] 路径: {}", id, path);

            PluginIndex? index = null;
            PluginInfo? info = null;
            PluginCheckResult result;

            try
            {
                index = GetIndex(path);

                Ensure.NotEmpty(index.Authors, nameof(index.Authors));

                Ensure.True(
                    index.Authors.All(
                        (author) => _userChecker.Exists(author.Name).GetAwaiter().GetResult()
                    ),
                    "存在不正确的用户"
                );
                _logger.LogInformation("[{}] 作者检查通过", id);

                info = await _rawContentHttpClient.GetPluginInfo(
                   index.Owner,
                   index.RepoName,
                   index.Branch,
                   index.Path
               );

                Ensure.True(id == info.Id, "插件Id不匹配");
                _logger.LogInformation("[{}] 插件Id检查通过", id);

                Ensure.NotNullOrEmpty(info.Name, nameof(info.Name));
                _logger.LogInformation("[{}] 插件名称检查通过", id);

                Ensure.NotEmpty(info.TargetingSerein, nameof(info.TargetingSerein));
                _logger.LogInformation("[{}] 适用版本检查通过", id);

                Ensure.NotEmpty(info.Tags, nameof(info.Tags));
                _logger.LogInformation("[{}] 插件标签检查通过", id);

                result = new(id, true, index, info);
            }
            catch (Exception e)
            {
                _logger.LogDebug(e, "[{}] 插件检查失败: {}", id, e);
                _logger.LogError(e, "[{}] 插件检查失败: {}", id, $"{e.GetType()}: {e.Message}");
                result = new(id, false, index, info, $"{e.GetType()}: {e.Message}");
            }

            list.Add(result);
        }

        return list;
    }

    private PluginIndex GetIndex(string path)
    {
        var jsonPath = Path.Combine(path, "plugin-index.json");

        var index =
            JsonSerializer.Deserialize<PluginIndex>(File.ReadAllText(jsonPath), JsonSerializerOptionsFactory.Json)
            ?? throw new CheckFailureExceptionException("Json文件为空");

        _logger.LogDebug("插件索引JSON: {}", JsonSerializer.Serialize(index));
        return index;
    }
}
