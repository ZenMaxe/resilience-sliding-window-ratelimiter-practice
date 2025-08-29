using System.ComponentModel.DataAnnotations;

namespace RateLimiter.SlidingWindowPractice.Infrastructure.Web.Options;

public sealed class ConnectionStringsOption
{
    public const string Key = "ConnectionStrings";

    [Required]
    public string Redis { get; set; }
}