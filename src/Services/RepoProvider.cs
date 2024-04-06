using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Octokit;

namespace ExtensionsHelper.Services;

public class RepoProvider
{
    public IReadOnlyDictionary<string, Repository> Repos => _repos;
    private readonly GitHubClient _gitHubClient;
    private readonly Dictionary<string, Repository> _repos;

    public RepoProvider(GitHubClient gitHubClient)
    {
        _gitHubClient = gitHubClient;
        _repos = new();
    }

    public async Task<Repository> GetWithCache(string owner, string name)
    {
        if (_repos.TryGetValue($"{owner}/{name}", out var repo))
            return repo;

        repo = await _gitHubClient.Repository.Get(owner, name);
        _repos[$"{owner}/{name}"] = repo;

        return repo;
    }
}
