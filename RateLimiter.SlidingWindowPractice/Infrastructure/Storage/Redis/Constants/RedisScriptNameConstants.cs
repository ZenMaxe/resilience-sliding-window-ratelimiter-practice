namespace RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Constants;

public static class RedisScriptNameConstants
{
    internal const string BasePath = "Infrastructure.Storage.Redis.Scripts.";


    public const string SlidingWindow = BasePath + "sliding_window.lua";
}