using StackExchange.Redis;

namespace RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Interfaces;

public interface IRedisScriptRegistry
{
    Task<LoadedLuaScript> GetLuaScriptAsync(string name);
}