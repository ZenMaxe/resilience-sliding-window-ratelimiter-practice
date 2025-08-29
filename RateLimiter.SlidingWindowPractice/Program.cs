using Polly;
using RateLimiter.SlidingWindowPractice.Core.Interfaces;
using RateLimiter.SlidingWindowPractice.Core.Services;
using RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis;
using RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Interfaces;
using RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Resilience;
using RateLimiter.SlidingWindowPractice.Infrastructure.Web.Interfaces;
using RateLimiter.SlidingWindowPractice.Infrastructure.Web.Middlewares;
using RateLimiter.SlidingWindowPractice.Infrastructure.Web.Models;
using RateLimiter.SlidingWindowPractice.Infrastructure.Web.Options;

namespace RateLimiter.SlidingWindowPractice;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();


        builder.Services.AddSingleton<IAsyncPolicy<bool>>(sp =>
            RedisPollyPolicy.CreateRedisPolicy(sp.GetRequiredService<ILoggerFactory>()));

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IRequestContext, RequestContext>();

        builder.Services.AddScoped<IRateLimiter, SlidingWindowRateLimiter>();

        builder.Services.AddSingleton<IRateLimitCacheStore, RedisRateLimitCacheStore>();
        builder.Services.AddSingleton<IRedisScriptRegistry, RedisScriptRegistry>();


        builder.Services
            .AddOptions<ConnectionStringsOption>()
            .Bind(builder.Configuration.GetSection(ConnectionStringsOption.Key))
            .ValidateDataAnnotations()
            .ValidateOnStart();


        var options = builder.Configuration
            .GetSection(ConnectionStringsOption.Key)
            .Get<ConnectionStringsOption>();

        RedisConnectionFactory.Initialize(new Uri(options!.Redis));



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.UseMiddleware<RateLimitMiddleware>();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}