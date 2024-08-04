namespace kokshengbi.Application.Common.Interfaces.Services
{
    public interface IRedisRateLimiterService
    {
        Task<bool> IsAllowedAsync(string key, int limit, TimeSpan period);
    }
}
