using System.Text.Json;
using Polly;
using RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Constants;
using RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Interfaces;
using StackExchange.Redis;

namespace RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis;

internal sealed class RedisRateLimitCacheStore : IRateLimitCacheStore
{
    private readonly IDatabase _database;
    private readonly IRedisScriptRegistry _redisScriptRegistry;
    private readonly IAsyncPolicy<bool> _redisPolicy;

    public RedisRateLimitCacheStore(IRedisScriptRegistry redisScriptRegistry, IAsyncPolicy<bool> redisPolicy)
    {
        _redisScriptRegistry = redisScriptRegistry;
        _redisPolicy = redisPolicy;
        _database = RedisConnectionFactory
            .GetConnectionMultiplexer()
            .GetDatabase();
    }

    public async Task<bool> IsAllowedAsync(string key, string randomId,
        int limit, TimeSpan window,
        CancellationToken cancellationToken)
    {

        var allowed = await _redisPolicy.ExecuteAsync(async (ct) =>
        {
            var script = await _redisScriptRegistry.GetLuaScriptAsync(RedisScriptNameConstants.SlidingWindow);



            var result = (int)await script.EvaluateAsync(
                _database,
                new
                {
                    key = (RedisKey)"rate_limit::1",
                    now = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    window = (long)window.TotalSeconds,
                    limit = limit,
                    uuid = randomId
                });


            return result == 1;
        }, cancellationToken);


        return allowed;
    }
}