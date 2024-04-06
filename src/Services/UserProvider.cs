using System.Collections.Generic;
using System.Threading.Tasks;

using Octokit;

namespace ExtensionsHelper.Services;

public class UserProvider
{
    public IReadOnlyDictionary<string, User?> Users => _users;
    private readonly GitHubClient _gitHubClient;
    private readonly Dictionary<string, User?> _users;

    public UserProvider(GitHubClient gitHubClient)
    {
        _gitHubClient = gitHubClient;
        _users = new();
    }

    public async Task<bool> Exists(string name, bool useCache = true)
    {
        if (useCache && _users.TryGetValue(name, out User? user) && user is not null)
            return true;

        try
        {
            _users[name] = await _gitHubClient.User.Get(name);
            return true;
        }
        catch
        {
            _users[name] = null;
            return false;
        }
    }

    public async Task<User> Get(string name, bool useCache = true)
    {
        if (useCache && _users.TryGetValue(name, out User? user) && user is not null)
            return user;

        try
        {
            user = await _gitHubClient.User.Get(name);
            _users[name] = user;
            return user;
        }
        catch
        {
            throw;
        }
    }
}
