using StackExchange.Redis;

namespace RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis;

// Simple Redis Connection Factory
internal static class RedisConnectionFactory
{
    private static ConnectionMultiplexer? _connectionMultiplexer;

    private static IServer? _server;

    public static ConnectionMultiplexer GetConnectionMultiplexer()
    {
        ArgumentNullException.ThrowIfNull(_connectionMultiplexer);
        return _connectionMultiplexer;
    }


    public static IServer GetServer()
    {
        ArgumentNullException.ThrowIfNull(_server);
        return _server;
    }

    public static void Initialize(Uri uri)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(uri.ToString());

        // pick the first endpoint
        var endpoint = _connectionMultiplexer.GetEndPoints().First();
        _server = _connectionMultiplexer.GetServer(endpoint);
    }
}