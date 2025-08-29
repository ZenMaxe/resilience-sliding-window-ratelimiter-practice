using RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Interfaces;
using StackExchange.Redis;

namespace RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis;

public class RedisScriptRegistry : IRedisScriptRegistry
{
    private static Dictionary<string, LoadedLuaScript> _registry = new();

    public async Task<LoadedLuaScript> GetLuaScriptAsync(string name)
    {
        if (_registry.TryGetValue(name, out var script))
        {
            return script;
        }

        var loadedScript = await RedisScriptLoader.LoadScriptAsync(name);
        _registry[name] = loadedScript;
        return loadedScript;
    }

}