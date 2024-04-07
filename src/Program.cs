using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Text;
using System.Threading.Tasks;

using ExtensionsHelper.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExtensionsHelper;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        return await CreateParser().InvokeAsync(args);
    }

    private static Parser CreateParser()
    {
        var rootCommnad = new RootCommand("Serein的扩展命令行工具");
        var checkCommand = new Command("check", "检查所有索引文件");

        var pathArgument = new Argument<string>("path", "SereinCommnunity/gallery 仓库路径");
        var tokenOption = new Option<string?>(new[] { "-t", "--token" }, "GitHub Token");
        var actionOption = new Option<bool>(new[] { "-a", "--action" }, "GitHub Action环境");
        var logLevelOption = new Option<LogLevel>(new[] { "-l", "--logLevel" }, () => LogLevel.Information, "输出等级");

        checkCommand.AddOption(tokenOption);
        checkCommand.AddOption(actionOption);
        checkCommand.AddOption(logLevelOption);
        checkCommand.AddArgument(pathArgument);
        checkCommand.SetHandler(Check, pathArgument, tokenOption, actionOption, logLevelOption);

        rootCommnad.AddCommand(checkCommand);

        return new CommandLineBuilder(rootCommnad)
            .UseExceptionHandler(WriteError)
            .UseTypoCorrections()
            .UseVersionOption()
            .UseHelp()
            .UseTypoCorrections()
            .UseParseErrorReporting()
            .RegisterWithDotnetSuggest()
            .Build();
    }

    private static void WriteError(Exception e, InvocationContext? context)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.ToString());
        Console.ReadLine();
        Console.ResetColor();

        if (e.HResult != 0 && context is not null)
            context.ExitCode = e.HResult;
    }

    private static async Task Check(string path, string? token, bool inGitHubAction, LogLevel logLevel)
    {
        var host = new AppBuilder(new()
        {
            InGitHubAction = inGitHubAction,
            Path = path,
            Token = token,
            LogLevel = logLevel
        }).Build();

        host.Start();
        await host.Services.GetRequiredService<PluginsManager>().Check();
    }
}
