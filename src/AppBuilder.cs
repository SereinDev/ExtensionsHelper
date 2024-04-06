using ExtensionsHelper.Models;
using ExtensionsHelper.Services;
using ExtensionsHelper.Utils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Octokit;

namespace ExtensionsHelper;

public sealed class AppBuilder
{
    private IServiceCollection Services => _hostAppBuilder.Services;

    private readonly HostApplicationBuilder _hostAppBuilder;

    public AppBuilder(Config config)
    {
        _hostAppBuilder = new HostApplicationBuilder();

        Services.AddSingleton(config);

        Services.AddSingleton<RawContentHttpClient>();
        Services.AddSingleton<PluginsManager>();
        Services.AddSingleton<UserProvider>();

        Services.AddSingleton<ILogger, CliLogger>((services) => new CliLogger(config));
        Services.AddSingleton(
            new GitHubClient(new ProductHeaderValue(nameof(ExtensionsHelper)))
            {
                Credentials = string.IsNullOrEmpty(config.Token)
                    ? Credentials.Anonymous
                    : new Credentials(config.Token)
            }
        );
    }

    public IHost Build() => _hostAppBuilder.Build();
}
