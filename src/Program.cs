using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsHelper;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        await CreateParser().InvokeAsync(args);
    }

    private static Parser CreateParser()
    {
        var rootCommnad = new RootCommand("Serein的扩展命令行工具");
        var checkCommand = new Command("check", "检查所有索引文件");

        var pathArgument = new Argument<string>("path", "SereinCommnunity/gallery 仓库路径");
        var tokenOption = new Option<string>(new[] { "-t", "--token" }, "GitHub Token");

        checkCommand.AddArgument(pathArgument);
        rootCommnad.AddOption(tokenOption);
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
}
