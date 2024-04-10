using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using ExtensionsHelper.Models.Plugins;

using Microsoft.Extensions.Logging;

using Octokit;

using Spectre.Console;

namespace ExtensionsHelper.Services;

public class SummaryGenerator
{
    private const string VAR = "GITHUB_STEP_SUMMARY";
    private readonly ILogger _logger;
    private readonly GitHubClient _gitHubClient;

    public SummaryGenerator(ILogger logger, GitHubClient gitHubClient)
    {
        _logger = logger;
        _gitHubClient = gitHubClient;
    }


    private async Task WriteAsync(string text, bool overwrite = false)
    {
        var path = Environment.GetEnvironmentVariable(VAR);

        _logger.LogDebug("Summary path: \r\n{}", path);
        _logger.LogDebug("Result: \r\n{}", text);

        if (!File.Exists(path))
            throw new InvalidOperationException();

        if (overwrite)
            await File.WriteAllTextAsync(path, text);
        else
            await File.AppendAllTextAsync(path, text);
    }

    public async Task SumPluginCheckResult(PluginCheckResult[] pluginCheckResults)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<h2>检查结果</h2>");


        foreach (var result in pluginCheckResults)
        {
            sb.AppendLine($"<h3>{result.Id}</h3>");
            sb.AppendLine();

            sb.Append("<table>");

            sb.Append("<tr>");
            sb.Append("<td>检查结果</td>");
            sb.AppendFormat("<td>{0}</td>", result.Success ? "✅" : "❌");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>原因</td>");
            sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(result.Reason));
            sb.Append("</tr>");

            sb.Append("</table>");
        }

        var limit = await _gitHubClient.RateLimit.GetRateLimits();

        sb.AppendLine();
        sb.AppendLine($"{limit.Rate.Remaining}/{limit.Rate.Limit}");

        await WriteAsync(sb.ToString());
    }

    public async Task WriteOutput(IEnumerable<PluginCheckResult> pluginCheckResults)
    {
        var table = new Table().AddColumns("Id", "检查结果", "原因");

        foreach (var result in pluginCheckResults)
            table.AddRow(result.Id, result.Success ? "✅" : "❌", result.Reason ?? string.Empty);

        AnsiConsole.Write(table);
        var limit = await _gitHubClient.RateLimit.GetRateLimits();
        AnsiConsole.WriteLine($"Octokit Api Limit: {limit.Rate.Remaining}/{limit.Rate.Limit}");
    }
}
