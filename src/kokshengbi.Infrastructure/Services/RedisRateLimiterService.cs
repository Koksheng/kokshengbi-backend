using kokshengbi.Application.Common.Interfaces.Services;
using StackExchange.Redis;

namespace kokshengbi.Infrastructure.Services
{
    public class RedisRateLimiterService : IRedisRateLimiterService
    {
        private readonly IDatabase _database;

        public RedisRateLimiterService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> IsAllowedAsync(string key, int limit, TimeSpan period)
        {
            var currentCount = await _database.StringIncrementAsync(key);
            if (currentCount == 1)
            {
                await _database.KeyExpireAsync(key, period);
            }

            return currentCount <= limit;
        }
    }
}
