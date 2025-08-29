using System.Reflection;
using StackExchange.Redis;

namespace RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis;

public static class RedisScriptLoader
{
    public static async Task<LoadedLuaScript> LoadScriptAsync(string resourcePath)
    {
        var server = RedisConnectionFactory.GetServer();
        var lua = ReadEmbeddedResource(resourcePath);
        var prepared = LuaScript.Prepare(lua);
        return await prepared.LoadAsync(server);
    }

    private static string ReadEmbeddedResource(string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var fullName = assembly
            .GetManifestResourceNames()
            .FirstOrDefault(x => x.EndsWith(resourcePath, StringComparison.OrdinalIgnoreCase));

        if (fullName == null)
            throw new FileNotFoundException($"Lua script not found: {resourcePath}");

        using var stream = assembly.GetManifestResourceStream(fullName)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}